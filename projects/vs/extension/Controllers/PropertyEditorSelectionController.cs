﻿using System;
using Forces.Models;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Forces.ViewModels;

namespace Forces.Controllers
{
	public interface IVsServiceProvider
	{
		IVsUIShell GetUIShellService();
		ITrackSelection GetTrackSelectionService();
	}

	public class PropertyEditorSelectionController
	{
		private readonly SelectionModel _model;
		private readonly IVsServiceProvider _serviceProvider;
		private IVsWindowFrame _frame;

		public PropertyEditorSelectionController(SelectionModel model, IVsServiceProvider serviceProvider)
		{
			_model = model;
			_model.SelectedSceneViewNodeChanged += _model_SelectedSceneViewNodeChanged;
			_serviceProvider = serviceProvider;
		}

		private void _model_SelectedSceneViewNodeChanged(object sender, ISceneViewNode e)
		{
			OnSelectedNodeChanged(e);
		}

		private void OnSelectedNodeChanged(ISceneViewNode sceneViewNode)
		{
			if (sceneViewNode is SceneViewNode svn)
			{
				TrackSelection(new NodePropertiesModel(svn.Node));
			}

			if (sceneViewNode is SceneViewCamera svc)
			{
				TrackSelection(new CameraPropertiesModel(svc.Camera));
			}
		}

		private void TrackSelection(object selection)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			if (_frame == null)
			{
				var shell = _serviceProvider.GetUIShellService();
				if (shell != null)
				{
					var guidPropertyBrowser = new Guid(ToolWindowGuids.PropertyBrowser);
					shell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guidPropertyBrowser, out _frame);
				}
			}

			_frame?.Show();
			var mySelContainer = new SelectionContainer()
			{
				SelectedObjects = new System.Collections.ArrayList()
				{
					selection
				}
			};

			var track = _serviceProvider.GetTrackSelectionService();
			track?.OnSelectChange(mySelContainer);
		}
	}
}

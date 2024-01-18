using System;
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

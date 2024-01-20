using System;
using System.Reactive.Linq;
using Forces.Models;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Forces.ViewModels;
using ReactiveUI;

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
			_serviceProvider = serviceProvider;
			ThreadHelper.ThrowIfNotOnUIThread();
			var shell = _serviceProvider.GetUIShellService();
			if (shell != null)
			{
				var guidPropertyBrowser = new Guid(ToolWindowGuids.PropertyBrowser);
				shell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guidPropertyBrowser, out var frame);
				frame?.Show();
			}
			model
				.WhenAnyValue(x => x.SelectedSceneViewNode)
				.Where(x => x is Node)
				.Select(x => new NodePropertiesModel(x as Node))
				.Subscribe(TrackSelection);
			model
				.WhenAnyValue(x => x.SelectedSceneViewNode)
				.Where(x => x is PreviewCamera)
				.Select(x => new CameraPropertiesModel(x as PreviewCamera))
				.Subscribe(TrackSelection);
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

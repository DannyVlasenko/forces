using System;
using System.Reactive.Linq;
using Forces.Models;
using Forces.Models.SceneTree;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Forces.ViewModels.PropertyEditor;
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
		private readonly IVsServiceProvider _serviceProvider;

		public PropertyEditorSelectionController(SelectionModel model, IVsServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			model
				.WhenAnyValue(x => x.SelectedSceneViewNode)
				.Select(CreateViewModelForModel)
				.WhereNotNull()
				.Subscribe(TrackSelection);
		}

		private void ShowPropertyEditor()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			var shell = _serviceProvider.GetUIShellService();
			if (shell == null) return;
			var guidPropertyBrowser = new Guid(ToolWindowGuids.PropertyBrowser);
			shell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guidPropertyBrowser, out var frame);
			frame?.Show();
		}

		private object CreateViewModelForModel(ModelObjectWithNotifications model)
		{
			switch (model)
			{
				case EmptyNode en:
					return new EmptyNodePropertyEditorViewModel(en);
				case MeshNode mn:
					return new MeshNodePropertyEditorViewModel(mn);
				case CameraNode cn:
					return new CameraNodePropertyEditorViewModel(cn);
				case LightNode ln:
					return new LightNodePropertyEditorViewModel(ln);
				case PreviewCamera pc:
					return new PreviewCameraPropertyEditorViewModel(pc);
				case AmbientLight al:
					return new AmbientLightPropertyEditorViewModel(al);
				case DirectedLight dl:
					return new DirectedLightPropertyEditorViewModel(dl);
			}

			return null;
		}

		private void TrackSelection(object selection)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			ShowPropertyEditor();
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

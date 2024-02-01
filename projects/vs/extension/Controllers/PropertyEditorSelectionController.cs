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
		private IVsUIShell _shell;
		private IVsWindowFrame _frame;

		public PropertyEditorSelectionController(SelectionModel model, IVsServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			ThreadHelper.ThrowIfNotOnUIThread();
			model
				.WhenAnyValue(x => x.SelectedSceneViewNode)
				.Select(CreateViewModelForModel)
				.WhereNotNull()
				.Subscribe(TrackSelection);
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
			var mySelContainer = new SelectionContainer()
			{
				SelectedObjects = new System.Collections.ArrayList()
				{
					selection
				}
			};

			if (_shell == null)
			{
				_shell = _serviceProvider.GetUIShellService();
				if (_shell == null) return;
				var guidPropertyBrowser = new Guid(ToolWindowGuids.PropertyBrowser);

				if (_frame == null)
				{
					_shell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guidPropertyBrowser, out _frame);
					_frame?.Show();
				}
			}

			if (_frame != null && _frame.IsVisible() != 0)
			{
				_frame.Show();
			}

			var track = _serviceProvider.GetTrackSelectionService();
			track?.OnSelectChange(mySelContainer);
		}
	}
}

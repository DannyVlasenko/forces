using System.Runtime.InteropServices;
using Forces.Controllers;
using Forces.Engine;
using Forces.Models;
using Forces.ViewModels;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Forces.Windows
{
	[Guid("606da3ec-a27c-4e36-b3fd-4d13f23204df")]
	public sealed class SceneViewWindow : ToolWindowPane, IVsServiceProvider
	{
		private readonly PropertyEditorSelectionController _propertyEditorSelectionController;

		public SceneViewWindow(SelectionModel model) : base(null)
		{
			_propertyEditorSelectionController = new PropertyEditorSelectionController(model, this);
			Content = new SceneViewWindowControl(new SceneViewModel(model)); 
			Caption = "Forces Scene";
		}

		public IVsUIShell GetUIShellService()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			return (IVsUIShell)GetService(typeof(SVsUIShell));
		}

		public ITrackSelection GetTrackSelectionService()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			return (ITrackSelection)GetService(typeof(STrackSelection));
		}
	}
}

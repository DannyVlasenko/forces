using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Forces.Windows;
using Forces.Controllers;
using Forces.Models;
using Microsoft.VisualStudio.Shell.Interop;

namespace Forces
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[Guid(ForcesPackage.PackageGuidString)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideToolWindow(typeof(SceneViewWindow))]
	[ProvideOptionPage(typeof(Preferences), "Forces", "General", 101, 106, true)]
	[ProvideToolWindow(typeof(PreviewWindow))]
	public sealed class ForcesPackage : AsyncPackage, IVsServiceProvider
	{
		public const string PackageGuidString = "c74f952b-7a75-440e-a270-264d3951d486";

		#region Package Members

		private readonly SelectionModel _selectionModel = new SelectionModel();
		private readonly SolutionExplorerSelectionController _solutionSelectionController;
		private readonly PropertyEditorSelectionController _propertyEditorSelectionController;

		public ForcesPackage()
		{
			_solutionSelectionController = new SolutionExplorerSelectionController(_selectionModel);
			_propertyEditorSelectionController = new PropertyEditorSelectionController(_selectionModel, this);
		}

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
		    await SceneViewWindowCommand.InitializeAsync(this);
		    await PreviewWindowCommand.InitializeAsync(this);
		    await OptionsCommand.InitializeAsync(this);
		}

		protected override WindowPane InstantiateToolWindow(Type toolWindowType)
		{
			if (toolWindowType == typeof(PreviewWindow) || toolWindowType == typeof(SceneViewWindow))
			{
				return base.InstantiateToolWindow(toolWindowType, _selectionModel);
			}

			return base.InstantiateToolWindow(toolWindowType);
		}

		#endregion

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

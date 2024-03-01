using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Forces.Commands;
using Forces.Windows;
using Forces.Controllers;
using Forces.Models;
using Microsoft.VisualStudio.Shell;

namespace Forces
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[Guid(PackageGuidString)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideToolWindow(typeof(SceneViewWindow))]
	[ProvideOptionPage(typeof(Preferences), "Forces", "General", 101, 106, true)]
	[ProvideToolWindow(typeof(PreviewWindow))]
	public sealed class ForcesPackage : AsyncPackage
	{
		public const string PackageGuidString = "c74f952b-7a75-440e-a270-264d3951d486";

		private readonly SelectionModel _selectionModel = new SelectionModel();
		private readonly SceneFileModel _sceneFileModel = new SceneFileModel();
		private readonly SolutionExplorerSelectionController _solutionSelectionController;

		public ForcesPackage()
		{
			_solutionSelectionController = new SolutionExplorerSelectionController(_selectionModel, _sceneFileModel);
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
			if (toolWindowType == typeof(PreviewWindow))
			{
				return base.InstantiateToolWindow(toolWindowType, _selectionModel);
			}

			if (toolWindowType == typeof(SceneViewWindow))
			{
				return base.InstantiateToolWindow(toolWindowType, _selectionModel);
			}

			return base.InstantiateToolWindow(toolWindowType);
		}
	}
}

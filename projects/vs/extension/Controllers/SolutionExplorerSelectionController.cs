using System.Diagnostics;
using System.IO;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using Forces.Models;

namespace Forces.Controllers
{
	public class SolutionExplorerSelectionController
	{
		private readonly DTE2 _dte2;
		private readonly SelectionModel _selectionModel;
		private readonly SceneFileModel _sceneFileModel;
		private readonly AssetFileModel _assetFileModel;
		private readonly Events2 _events;
		private readonly SelectionEvents _selectionEvents;

		public SolutionExplorerSelectionController(SelectionModel selectionModel, SceneFileModel sceneFileModel, AssetFileModel assetFileModel)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			_dte2 = ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as DTE2;
			_selectionModel = selectionModel;
			_sceneFileModel = sceneFileModel;
			_assetFileModel = assetFileModel;
			_events = _dte2?.Events as Events2;
			_selectionEvents = _events?.SelectionEvents;
			_events.WindowVisibilityEvents.WindowShowing += WindowVisibilityEvents_WindowShowing;
			Debug.Assert(_selectionEvents != null, nameof(_selectionEvents) + " != null");
			_selectionEvents.OnChange += _selectionEvents_OnChange;
		}

		private void WindowVisibilityEvents_WindowShowing(Window window)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			if (window?.Document == null) { return; }

			var extension = Path.GetExtension(window.Document.FullName);
			if (extension == ".fsc" || extension == ".fas")
			{
				window.Close();
			}
		}

		private void _selectionEvents_OnChange()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			if (!_dte2.SelectedItems.MultiSelect)
			{
				var selected = (UIHierarchyItem[])_dte2.ToolWindows.SolutionExplorer.SelectedItems;
				if (selected.Length == 1 && Path.GetExtension(selected[0].Name) == ".fsc")
				{
					var scene = _sceneFileModel.SceneForFile(selected[0].Name);
					_selectionModel.SelectedScene = scene;
				}
				if (selected.Length == 1 && Path.GetExtension(selected[0].Name) == ".fas")
				{
					var assetStorage = _assetFileModel.AssetStorageForFile(selected[0].Name);
					_selectionModel.SelectedAssetStorage = assetStorage;
				}
			}
		}
	}
}

using System.IO;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using Forces.Engine;
using Forces.Models;

namespace Forces.Controllers
{
	public class SolutionExplorerSelectionController
	{
		private readonly DTE2 _dte2;
		private readonly SelectedSceneModel _sceneModel;
		private readonly Events2 _events;
		private readonly SelectionEvents _selectionEvents;

		public SolutionExplorerSelectionController(SelectedSceneModel sceneModel)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			_dte2 = ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as DTE2;
			_sceneModel = sceneModel;
			_events = _dte2.Events as Events2;
			_selectionEvents = _events.SelectionEvents;
			_selectionEvents.OnChange += _selectionEvents_OnChange;
		}

		private void _selectionEvents_OnChange()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			if (!_dte2.SelectedItems.MultiSelect)
			{
				var selected = (UIHierarchyItem[])_dte2.ToolWindows.SolutionExplorer.SelectedItems;
				if (selected.Length == 1 && Path.GetExtension(selected[0].Name) == ".fsc")
				{
					//Load scene from file
					_sceneModel.SelectedScene = new Scene();
				}
			}
		}
	}
}

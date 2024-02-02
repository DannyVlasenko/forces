using System.Reactive;
using ReactiveUI;

namespace Forces.ViewModels.SceneView
{
	public class NodeAction
	{
		public NodeAction(string actionName, ReactiveCommand<Unit, Unit> actionCommand)
		{
			ActionName = actionName;
			ActionCommand = actionCommand;
		}

		public string ActionName { get; }

		public ReactiveCommand<Unit, Unit> ActionCommand { get; }
	}
}
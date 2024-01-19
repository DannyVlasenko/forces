namespace Forces.Models
{
	public class SelectionModel : ModelObjectWithNotifications
	{
		private Node _selectedSceneViewNode;
		private Scene _selectedScene;

		public Node SelectedSceneViewNode
		{
			get => _selectedSceneViewNode;
			set => SetField(ref _selectedSceneViewNode, value);
		}

		public Scene SelectedScene
		{
			get => _selectedScene;
			set => SetField(ref _selectedScene, value);
		}
	}
}

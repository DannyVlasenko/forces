namespace Forces.Models
{
	public class SelectionModel : ModelObjectWithNotifications
	{
		private ModelObjectWithNotifications _selectedSceneViewNode;
		private Scene _selectedScene;

		public ModelObjectWithNotifications SelectedSceneViewNode
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

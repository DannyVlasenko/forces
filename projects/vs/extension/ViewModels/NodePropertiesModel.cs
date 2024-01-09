using Forces.Engine;
using Forces.Models;

namespace Forces.ViewModels
{
	internal class NodePropertiesModel
	{
		private readonly Node _node;
		private readonly SelectedSceneModel _sceneModel;

		public NodePropertiesModel(Node node, SelectedSceneModel sceneModel)
		{
			_node = node;
			_sceneModel = sceneModel;
		}

		public string Name => _node.Name;

		public bool IsVisible => _node.IsVisible;

		public Vec3 Translation
		{
			get => _node.Translation;
			set
			{
				_node.Translation = value;
				_sceneModel.UpdateScene(_sceneModel.SelectedScene, _sceneModel.SceneName);
			}
		}

		public override string ToString() => Name;
	}
}

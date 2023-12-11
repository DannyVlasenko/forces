using Forces.Engine;

namespace Forces.ViewModels
{
	internal class NodePropertiesModel
	{
		private readonly Node _node;

		public NodePropertiesModel(Node node)
		{
			_node = node;
		}

		public string Name => _node.Name;

		public bool IsVisible => _node.IsVisible;

		public override string ToString() => Name;
	}
}

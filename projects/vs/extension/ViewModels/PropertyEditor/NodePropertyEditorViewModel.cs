using Forces.Models;
using Forces.Models.SceneTree;
using Forces.Utilities;

namespace Forces.ViewModels.PropertyEditor
{
	public abstract class NodePropertyEditorViewModel
	{
		private readonly Node _node;

		protected NodePropertyEditorViewModel(Node node)
		{
			_node = node;
		}

		public string Name
		{
			get => _node.Name;
			set => _node.Name = value;
		}

		public Vector3Property Translation => new Vector3Property(() => _node.Translation, tr => _node.Translation = tr);

		public Vector3Property Scale => new Vector3Property(() => _node.Scale, sc => _node.Scale = sc);

		public Vector3Property Rotation => new Vector3Property(() => _node.Rotation.ToEulerXYZ(), rt => _node.Rotation = rt.EulerXYZToQuaternion());

		public override string ToString() => Name;
	}
}
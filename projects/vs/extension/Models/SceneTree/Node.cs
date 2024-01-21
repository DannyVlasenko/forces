using System.Collections.ObjectModel;
using System.Numerics;

namespace Forces.Models.SceneTree
{
	public abstract class Node : ModelObjectWithNotifications
	{
		private string _name;
		private Vector3 _translation = Vector3.Zero;
		private Vector3 _scale = Vector3.One;
		private Quaternion _rotation = Quaternion.Identity;

		protected Node(string name)
		{
			_name = name;
		}

		public string Name
		{
			get => _name;
			set => SetField(ref _name, value);
		}

		public Vector3 Translation
		{
			get => _translation;
			set => SetField(ref _translation, value);
		}

		public Vector3 Scale
		{
			get => _scale;
			set => SetField(ref _scale, value);
		}

		public Quaternion Rotation
		{
			get => _rotation;
			set => SetField(ref _rotation, value);
		}

		public ObservableCollection<Node> Children { get; } = new ObservableCollection<Node>();
	}
}

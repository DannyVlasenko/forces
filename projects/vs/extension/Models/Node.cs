using System.Collections.ObjectModel;
using System.Numerics;

namespace Forces.Models
{
	public abstract class Node : ModelObjectWithNotifications
	{
		private string _name;
		private Vector3 _translation = new Vector3(0.0f);
		private Vector3 _scale = new Vector3(1.0f);
		private Vector3 _rotation = new Vector3(0.0f);

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

		public Vector3 Rotation
		{
			get => _rotation;
			set => SetField(ref _rotation, value);
		}

		public ObservableCollection<Node> Children { get; } = new ObservableCollection<Node>();
	}
}

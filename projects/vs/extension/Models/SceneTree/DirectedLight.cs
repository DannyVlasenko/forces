using System.Drawing;
using System.Numerics;

namespace Forces.Models.SceneTree
{
	public class DirectedLight : Light
	{
		private Vector3 _direction;
		private string _name;

		public DirectedLight(Color color, Vector3 direction, string name) : base(color)
		{
			_direction = direction;
			_name = name;
		}

		public Vector3 Direction
		{
			get => _direction;
			set => SetField(ref _direction, value);
		}

		public string Name
		{
			get => _name;
			set => SetField(ref _name, value);
		}
	}
}

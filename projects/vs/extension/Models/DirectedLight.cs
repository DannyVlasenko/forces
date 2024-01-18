using System.Drawing;
using System.Numerics;

namespace Forces.Models
{
	public class DirectedLight : Light
	{
		private Vector3 _direction;
		public DirectedLight(Color color, Vector3 direction) : base(color)
		{
			_direction = direction;
		}
		public Vector3 Direction
		{
			get => _direction;
			set => SetField(ref _direction, value);
		}
	}
}

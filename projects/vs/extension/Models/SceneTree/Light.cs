using System.Drawing;

namespace Forces.Models.SceneTree
{
	public abstract class Light : ModelObjectWithNotifications
	{
		private Color _color;

		protected Light(Color color)
		{
			_color = color;
		}

		public Color Color
		{
			get => _color;
			set => SetField(ref _color, value);
		}
	}
}

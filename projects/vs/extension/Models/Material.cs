using System.Drawing;

namespace Forces.Models
{
	public class Material : ModelObjectWithNotifications
	{
		private string _name;
		private Color _color;

		public Material(string name, Color color)
		{
			_name = name;
			_color = color;
		}

		public string Name
		{
			get => _name;
			set => SetField(ref _name, value);
		}

		public Color Color
		{
			get => _color;
			set => SetField(ref _color, value);
		}
	}
}

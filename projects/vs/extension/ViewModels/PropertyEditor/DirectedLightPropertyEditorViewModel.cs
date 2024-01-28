using System.Drawing;
using Forces.Models.SceneTree;

namespace Forces.ViewModels.PropertyEditor
{
	public class DirectedLightPropertyEditorViewModel
	{
		private readonly DirectedLight _light;

		public DirectedLightPropertyEditorViewModel(DirectedLight light)
		{
			_light = light;
		}

		public string Name
		{
			get => _light.Name;
			set => _light.Name = value;
		}

		public Vector3Property Direction => new Vector3Property(() => _light.Direction, dir => _light.Direction = dir);

		public Color Color
		{
			get => _light.Color;
			set => _light.Color = value;
		}
	}
}
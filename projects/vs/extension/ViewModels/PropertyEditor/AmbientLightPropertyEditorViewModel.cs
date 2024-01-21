using System.Drawing;
using Forces.Models;
using Forces.Models.SceneTree;

namespace Forces.ViewModels.PropertyEditor
{
	public class AmbientLightPropertyEditorViewModel
	{
		private readonly AmbientLight _light;

		public AmbientLightPropertyEditorViewModel(AmbientLight light)
		{
			_light = light;
		}

		public Color Color
		{
			get => _light.Color;
			set => _light.Color = value;
		}
	}
}
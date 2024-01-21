using System.Drawing;
using Forces.Models;
using Forces.Models.SceneTree;

namespace Forces.ViewModels.PropertyEditor
{
	public class LightNodePropertyEditorViewModel : NodePropertyEditorViewModel
	{
		private readonly PointLight _pointLight;

		public LightNodePropertyEditorViewModel(LightNode node) : base(node)
		{
			_pointLight = node.Light;
		}

		public Color Color
		{
			get => _pointLight.Color;
			set => _pointLight.Color = value;
		}
	}
}
using System.Drawing;

namespace Forces.Models.SceneTree
{
	public class LightNode : Node
	{
		public LightNode(string name) : base(name) {}
		public PointLight Light { get; } = new PointLight(Color.White);
	}
}

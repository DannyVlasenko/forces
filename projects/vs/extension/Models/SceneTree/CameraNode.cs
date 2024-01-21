namespace Forces.Models.SceneTree
{
	public class CameraNode : Node
	{
		public CameraNode(string name) : base(name) {}

		public Camera Camera { get; } = new Camera();
	}
}

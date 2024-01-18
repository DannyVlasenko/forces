namespace Forces.Models
{
	public class CameraNode : Node
	{
		public CameraNode(string name) : base(name) {}

		private Camera Camera { get; } = new Camera();
	}
}

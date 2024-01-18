namespace Forces.Models
{
	public class Scene : ModelObjectWithNotifications
	{
		private Node RootNode { get; } = new EmptyNode("Root");
	}
}

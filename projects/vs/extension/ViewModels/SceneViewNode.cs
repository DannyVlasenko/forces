using System.Collections.Generic;
using System.Linq;
using Forces.Engine;

namespace Forces.ViewModels
{
	public interface ISceneViewNode
	{
		string Name { get; }
		IEnumerable<ISceneViewNode> Children { get; }
	}

	public class SceneViewNode : ISceneViewNode
	{
		public Node Node { get; }

		public SceneViewNode(Node node)
		{
			Node = node;
		}

		public string Name => Node.Name;
		public IEnumerable<ISceneViewNode> Children => Node.Children.Select(x => new SceneViewNode(x));
	}

	public class SceneViewCamera : ISceneViewNode
	{
		public Camera Camera { get; }

		public SceneViewCamera(Camera camera)
		{
			Camera = camera;
		}

		public string Name => "Camera";
		public IEnumerable<ISceneViewNode> Children => Enumerable.Empty<ISceneViewNode>();
	}
}

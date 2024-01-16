using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Forces.Engine;
using Microsoft.VisualStudio.PlatformUI;

namespace Forces.ViewModels
{
	public interface ISceneViewNode
	{
		string Name { get; }
		IEnumerable<ISceneViewNode> Children { get; }

		ICommand CreateNodeCommand { get; }
	}

	public class SceneViewNode : ISceneViewNode
	{
		public Node Node { get; }

		public SceneViewNode(Node node)
		{
			Node = node;

			CreateNodeCommand = new DelegateCommand(() => {
				var sphereNode = new Node(Node, "Sphere1");
				sphereNode.AddMesh(Node.Scene.Meshes[0]);
			});
		}

		public string Name
		{
			get => Node.Name;
			set => Node.Name = value;
		}

		public IEnumerable<ISceneViewNode> Children => Node.Children.Select(x => new SceneViewNode(x));
		public ICommand CreateNodeCommand { get; }
	}

	public class SceneViewCamera : ISceneViewNode
	{
		public Camera Camera { get; }

		public SceneViewCamera(Camera camera)
		{
			Camera = camera;
			CreateNodeCommand = new DelegateCommand(() => {});
		}

		public string Name => "Camera";
		public IEnumerable<ISceneViewNode> Children => Enumerable.Empty<ISceneViewNode>();
		public ICommand CreateNodeCommand { get; }
	}
}

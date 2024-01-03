using System.Collections.Generic;

namespace Forces.Engine
{
	internal class Scene
	{
		public Node RootNode { get; }
		public IList<Mesh> Meshes { get; private set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Forces.Engine
{
	internal class Node
	{
		public Node(string itemName)
		{
			Name = itemName;
			Children = new List<Node>();
			Meshes = new List<WeakReference<Mesh>>();
		}

		public string Name { get; set; }

		public bool IsVisible { get; set; }

		public override string ToString()
		{
			return Name;
		}

		public IList<Node> Children { get; private set; }
		public IList<WeakReference<Mesh>> Meshes { get; private set; }
	}
}

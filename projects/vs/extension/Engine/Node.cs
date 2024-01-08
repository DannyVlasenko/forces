using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	internal class Node
	{
		private readonly IntPtr _node;

		public IntPtr Handle => _node;

		public Node(IntPtr node, string name)
		{
			_node = node;
			Name = name;
		}

		public Node(Node parent, string itemName)
		{
			_node = create_node(parent._node);
			Name = itemName;
		}

		public void AddMesh(Mesh mesh)
		{
			node_add_mesh(_node, mesh.Handle);
		}

		public string Name { get; set; }

		public bool IsVisible { get; set; }

		public IReadOnlyList<Node> Children
		{
			get
			{
				var children = new List<Node>();
				return children.AsReadOnly();
			}
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_node(IntPtr parent);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_add_mesh(IntPtr node, IntPtr mesh);
	}
}

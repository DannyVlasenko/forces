using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	public class Node
	{
		private readonly IntPtr _node;

		public IntPtr Handle => _node;

		public Vec3 Translation
		{
			get => node_get_translation(_node);
			set => node_set_translation(_node, value);
		}

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
				var count = node_children_count(_node);
				var children = new IntPtr[count];
				var returned = node_get_children(_node, children, children.Length);
				return children
					.Take(returned)
					.Select(x=>new Node(x, "child"))
					.ToList();
			}
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_node(IntPtr parent);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_add_mesh(IntPtr node, IntPtr mesh);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern Vec3 node_get_translation(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_set_translation(IntPtr node, Vec3 translation);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern int node_children_count(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern int node_get_children(IntPtr node, IntPtr[] outChildren, int outChildrenCount);
	}
}

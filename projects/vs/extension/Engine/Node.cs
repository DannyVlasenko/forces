using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	public class Node
	{
		public IntPtr Handle { get; }

		public Vec3 Translation
		{
			get => node_get_translation(Handle);
			set => node_set_translation(Handle, value);
		}

		public Node(IntPtr node)
		{
			Handle = node;
		}

		public Node(Node parent, string nodeName)
		{
			Handle = create_node(parent.Handle, nodeName);
		}

		public void AddMesh(Mesh mesh)
		{
			node_add_mesh(Handle, mesh.Handle);
		}

		public string Name
		{
			get => Marshal.PtrToStringUni(node_get_name(Handle));
			set => node_set_name(Handle, value);
		}

		public bool IsVisible { get; set; }

		public IReadOnlyList<Node> Children
		{
			get
			{
				var count = node_children_count(Handle);
				var children = new IntPtr[count];
				var returned = node_get_children(Handle, children, children.Length);
				return children
					.Take(returned)
					.Select(x=>new Node(x))
					.ToList();
			}
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_node(IntPtr parent, string name);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_add_mesh(IntPtr node, IntPtr mesh);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern Vec3 node_get_translation(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_set_translation(IntPtr node, Vec3 translation);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr node_get_name(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_set_name(IntPtr node, string name);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern int node_children_count(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern int node_get_children(IntPtr node, IntPtr[] outChildren, int outChildrenCount);
	}
}

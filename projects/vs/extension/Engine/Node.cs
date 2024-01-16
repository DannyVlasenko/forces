using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	public class Node
	{
		public Scene Scene { get; }

		public IntPtr Handle { get; }

		public Vec3 Translation
		{
			get => node_get_translation(Handle);
			set
			{
				node_set_translation(Handle, value);
				Scene.NotifyNodeChanged(this);
			}
		}

		public Node(IntPtr node, string name, Scene scene)
		{
			Handle = node;
			Scene = scene;
			Name = name;
		}

		public Node(Node parent, string itemName)
		{
			Handle = create_node(parent.Handle);
			Name = itemName;
			Scene = parent.Scene;
		}

		public void AddMesh(Mesh mesh)
		{
			node_add_mesh(Handle, mesh.Handle);
			Scene.NotifyNodeChanged(this);
		}

		public string Name { get; set; }

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
					.Select(x=>new Node(x, "child", Scene))
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

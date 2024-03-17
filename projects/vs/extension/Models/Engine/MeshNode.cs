using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class MeshNode : Node
	{
		public MeshNode(Node parent, string nodeName, Mesh mesh, Material material) : 
			base(create_mesh_node(parent.Handle, nodeName, mesh.Handle, material.Handle))
		{}

		public Mesh Mesh
		{
			set => node_set_mesh(Handle, value.Handle);
		}

		public Material Material
		{
			set => node_set_material(Handle, value.Handle);
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_mesh_node(IntPtr parent, string name, IntPtr mesh, IntPtr material);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_set_mesh(IntPtr meshNode, IntPtr mesh);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_set_material(IntPtr meshNode, IntPtr material);
	}
}

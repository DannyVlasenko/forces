using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class MeshNode : Node
	{
		internal MeshNode(Node parent, string nodeName, Mesh mesh, Material material) : 
			base(create_mesh_node(parent.Handle, nodeName, mesh.Handle, material.Handle))
		{}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_mesh_node(IntPtr parent, string name, IntPtr mesh, IntPtr material);
	}
}

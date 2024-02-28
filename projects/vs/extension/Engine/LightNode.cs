using System;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	public class LightNode : Node
	{
		public LightNode(Node parent, string name) :
			base(create_light_node(parent.Handle, name))
		{ }
		public Vec3 Color
		{
			set => light_node_set_color(Handle, value);
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_light_node(IntPtr parent, string name);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void light_node_set_color(IntPtr material, Vec3 color);
	}
}

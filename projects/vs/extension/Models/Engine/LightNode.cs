using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class LightNode : Node
	{
		public LightNode(Node parent, string name) :
			base(create_light_node(parent.Handle, name))
		{ }
		
		public PointLight Light => new PointLight(node_get_light(Handle));

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_light_node(IntPtr parent, string name);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr node_get_light(IntPtr node);
	}
}

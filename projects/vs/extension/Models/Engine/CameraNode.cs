using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class CameraNode : Node
	{
		public CameraNode(Node parent, string name) :
			base(create_camera_node(parent.Handle, name))
		{ }

		public Camera Camera => new Camera(node_get_camera(Handle));

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_camera_node(IntPtr parent, string name);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr node_get_camera(IntPtr cameraNode);
	}
}
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
			set => node_set_translation(Handle, value);
		}

		public Vec3 Scale
		{
			set => node_set_scale(Handle, value);
		}

		public Vec4 Rotation
		{
			set => node_set_rotation(Handle, value);
		}

		public Node(IntPtr node)
		{
			Handle = node;
		}

		public string Name
		{
			set => node_set_name(Handle, value);
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern Vec3 node_get_translation(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_set_translation(IntPtr node, Vec3 translation);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern Vec3 node_get_scale(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_set_scale(IntPtr node, Vec3 scale);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern Vec4 node_get_rotation(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_set_rotation(IntPtr node, Vec4 rotation);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr node_get_name(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void node_set_name(IntPtr node, string name);
	}
}

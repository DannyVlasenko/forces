using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class AmbientLight
	{
		public IntPtr Handle { get; }

		public AmbientLight(IntPtr handle)
		{
			Handle = handle;
		}

		public Vec3 Color
		{
			set => ambient_light_set_color(Handle, value);
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void ambient_light_set_color(IntPtr handle, Vec3 color);
	}
}
using System.Runtime.InteropServices;
using System;

namespace Forces.Models.Engine
{
	public class Light
	{
		public IntPtr Handle { get; }

		public Light(IntPtr handle)
		{
			Handle = handle;
		}

		public Vec3 Color
		{
			set => light_set_color(Handle, value);
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void light_set_color(IntPtr handle, Vec3 color);
	}
}
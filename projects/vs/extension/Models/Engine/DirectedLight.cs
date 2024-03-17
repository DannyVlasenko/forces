using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class DirectedLight : Light
	{
		public DirectedLight(IntPtr handle) : base(handle)
		{}

		public string Name
		{
			set => directed_light_set_name(Handle, value);
		}

		public Vec3 Direction
		{
			set => directed_light_set_direction(Handle, value);
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void directed_light_set_name(IntPtr node, string name);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void directed_light_set_direction(IntPtr light, Vec3 direction);
	}
}
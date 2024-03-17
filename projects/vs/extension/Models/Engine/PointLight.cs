using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class PointLight : Light
	{
		public PointLight(IntPtr handle) : base(handle)
		{}

		public float Strength
		{
			set => point_light_set_strength(Handle, value);
		}


		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void point_light_set_strength(IntPtr node, float strength);
	}
}
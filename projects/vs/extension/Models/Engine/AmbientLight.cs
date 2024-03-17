using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class AmbientLight : Light
	{
		public AmbientLight(IntPtr handle) : base(handle)
		{}
	}
}
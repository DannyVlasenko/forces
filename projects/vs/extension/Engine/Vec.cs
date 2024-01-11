using System.Runtime.InteropServices;

namespace Forces.Engine
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vec2
	{
		public float X;
		public float Y;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct Vec3
	{
		public float X;
		public float Y;
		public float Z;
	};
}

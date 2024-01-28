using System.Numerics;
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

		private Vec3(Vector3 src)
		{
			X = src.X;
			Y = src.Y;
			Z = src.Z;
		}

		public static implicit operator Vector3(Vec3 a) => new Vector3(a.X, a.Y, a.Z);
		public static implicit operator Vec3(Vector3 a) => new Vec3(a);
	}
}

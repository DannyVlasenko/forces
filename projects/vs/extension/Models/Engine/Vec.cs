using System.Numerics;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vec2
	{
		public float X;
		public float Y;

		public Vec2(float x, float y)
		{
			X = x;
			Y = y;
		}
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct Vec3
	{
		public float X;
		public float Y;
		public float Z;

		public Vec3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		private Vec3(Vector3 src)
		{
			X = src.X;
			Y = src.Y;
			Z = src.Z;
		}

		public static implicit operator Vector3(Vec3 a) => new Vector3(a.X, a.Y, a.Z);
		public static implicit operator Vec3(Vector3 a) => new Vec3(a);
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vec4
	{
		public float W;
		public float X;
		public float Y;
		public float Z;

		private Vec4(Quaternion src)
		{
			W = src.W;
			X = src.X;
			Y = src.Y;
			Z = src.Z;
		}

		public static implicit operator Quaternion(Vec4 a) => new Quaternion(a.X, a.Y, a.Z, a.W);
		public static implicit operator Vec4(Quaternion a) => new Vec4(a);
	}
}

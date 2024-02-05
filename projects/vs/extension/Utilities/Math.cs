using System.Numerics;
using System;

namespace Forces.Utilities
{
	public static class Vector3Extensions
	{
		public static Quaternion EulerXYZToQuaternion(this Vector3 eulerXYZ)
		{
			var cx = (float)Math.Cos(eulerXYZ.X * 0.5);
			var cy = (float)Math.Cos(eulerXYZ.Y * 0.5);
			var cz = (float)Math.Cos(eulerXYZ.Z * 0.5);

			var sx = (float)Math.Sin(eulerXYZ.X * 0.5);
			var sy = (float)Math.Sin(eulerXYZ.Y * 0.5);
			var sz = (float)Math.Sin(eulerXYZ.Z * 0.5);

			return new Quaternion
			{
				W = (cx * cy * cz + sx * sy * sz),
				X = (sx * cy * cz - cx * sy * sz),
				Y = (cx * sy * cz + sx * cy * sz),
				Z = (cx * cy * sz - sx * sy * cz)
			};
		}
	}

	public static class QuaternionExtensions
	{
		public static Vector3 ToEulerXYZ(this Quaternion q)
		{
			var angles = new Vector3();

			// roll / x
			double sinRCosP = 2 * (q.W * q.X + q.Y * q.Z);
			double cosRCosP = 1 - 2 * (q.X * q.X + q.Y * q.Y);
			angles.X = (float)Math.Atan2(sinRCosP, cosRCosP);

			// pitch / y
			double sinP = 2 * (q.W * q.Y - q.Z * q.X);
			if (Math.Abs(sinP) >= 1)
			{
				angles.Y = (float)(Math.PI / 2.0 * Math.Sign(sinP));
			}
			else
			{
				angles.Y = (float)Math.Asin(sinP);
			}

			// yaw / z
			double sinYCosP = 2 * (q.W * q.Z + q.X * q.Y);
			double cosYCosP = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
			angles.Z = (float)Math.Atan2(sinYCosP, cosYCosP);

			return angles;
		}

		public static float ToRadians(this float angle)
		{
			return (float)(angle * Math.PI / 180.0f);
		}
	}
}
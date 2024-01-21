using System.Numerics;
using System;

namespace Forces.Utilities
{
	public static class Vector3Extensions
	{
		public static Quaternion EulerXYZToQuaternion(this Vector3 eulerXYZ)
		{
			var cy = (float)Math.Cos(eulerXYZ.Z * 0.5);
			var sy = (float)Math.Sin(eulerXYZ.Z * 0.5);
			var cp = (float)Math.Cos(eulerXYZ.Y * 0.5);
			var sp = (float)Math.Sin(eulerXYZ.Y * 0.5);
			var cr = (float)Math.Cos(eulerXYZ.X * 0.5);
			var sr = (float)Math.Sin(eulerXYZ.X * 0.5);

			return new Quaternion
			{
				W = (cr * cp * cy + sr * sp * sy),
				X = (sr * cp * cy - cr * sp * sy),
				Y = (cr * sp * cy + sr * cp * sy),
				Z = (cr * cp * sy - sr * sp * cy)
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
	}
}
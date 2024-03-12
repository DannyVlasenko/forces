using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public sealed class Camera
	{
		public Camera(IntPtr handle)
		{
			Handle = handle;
		}

		public IntPtr Handle { get; }

		public Vec2 Viewport
		{
			set => camera_set_viewport(Handle, value);
		}

		public float Near
		{
			set => camera_set_near(Handle, value);
		}

		public float Far
		{
			set => camera_set_far(Handle, value);
		}

		public float FOV
		{
			set => camera_set_fov(Handle, value);
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern Vec2 camera_get_viewport(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void camera_set_viewport(IntPtr node, Vec2 viewport);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern float camera_get_near(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void camera_set_near(IntPtr node, float near);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern float camera_get_far(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void camera_set_far(IntPtr node, float far);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern float camera_get_fov(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void camera_set_fov(IntPtr node, float fov);
	}
}

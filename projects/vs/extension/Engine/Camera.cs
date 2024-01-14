using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	public class Camera : IDisposable, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public IntPtr Handle { get; private set; } = create_camera();

		public Vec3 Position
		{
			get => camera_get_position(Handle);
			set
			{
				camera_set_position(Handle, value); 
				OnPropertyChanged();
			}
		}

		public Vec2 Viewport
		{
			get => camera_get_viewport(Handle);
			set
			{
				camera_set_viewport(Handle, value); 
				OnPropertyChanged();
			}
		}

		public float Near
		{
			get => camera_get_near(Handle);
			set
			{
				camera_set_near(Handle, value);
				OnPropertyChanged();
			}
		}

		public float Far
		{
			get => camera_get_far(Handle);
			set
			{
				camera_set_far(Handle, value);
				OnPropertyChanged();
			}
		}

		public float FOV
		{
			get => camera_get_fov(Handle);
			set
			{
				camera_set_fov(Handle, value);
				OnPropertyChanged();
			}
		}

		private void ReleaseUnmanagedResources()
		{
			delete_camera(Handle);
			Handle = IntPtr.Zero;
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~Camera()
		{
			ReleaseUnmanagedResources();
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_camera();

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void delete_camera(IntPtr camera);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern Vec3 camera_get_position(IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void camera_set_position(IntPtr node, Vec3 position);

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

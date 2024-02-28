using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Render
{
	public class Window : IDisposable
	{
		public IntPtr Handle { get; private set; }

		public Window(RenderWindow from)
		{
			Handle = create_render_window(from.Handle);
		}
		private void ReleaseUnmanagedResources()
		{
			delete_render_window(Handle);
			Handle = IntPtr.Zero;
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~Window()
		{
			ReleaseUnmanagedResources();
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_render_window(IntPtr glfwWindow);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void delete_render_window(IntPtr handle);
	}
}
using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Render
{
	public class Window : IDisposable
	{
		public IntPtr Handle { get; private set; }

		public Window(RenderWindow from)
		{
			Handle = adapt_glfw_window(from.Handle);
		}
		private void ReleaseUnmanagedResources()
		{
			delete_window_adapter(Handle);
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
		private static extern IntPtr adapt_glfw_window(IntPtr glfwWindow);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void delete_window_adapter(IntPtr handle);
	}
}
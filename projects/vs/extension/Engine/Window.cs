using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace Forces.Engine
{
	internal class Window : HwndHost
	{
		public OpenGLRenderer Renderer { get; set; }
		private IntPtr _hwnd;

		public void MakeContextCurrent()
		{
			glfwMakeContextCurrent(_hwnd);
		}

		public void SwapBuffers()
		{
			glfwSwapBuffers(_hwnd);
		}

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			glfwInit();
			glfwWindowHint(0x0002100D, 8);
			_hwnd = glfwCreateWindow(100, 100, "Forces Preview", IntPtr.Zero, IntPtr.Zero);
			var win32Handle = glfwGetWin32Window(_hwnd);
			const int GWL_STYLE = (-16);
			const int WS_CHILD = 0x40000000;
			SetWindowLong(win32Handle, GWL_STYLE, WS_CHILD);
			SetParent(win32Handle, hwndParent.Handle);
			MakeContextCurrent();
			Renderer = new OpenGLRenderer();
			return new HandleRef(this, win32Handle);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			if (!ReferenceEquals(hwnd.Wrapper, this)) return;
			glfwDestroyWindow(_hwnd);
			_hwnd = IntPtr.Zero;
			glfwTerminate();
		}

		protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 15)
			{
				Renderer?.Render();
				SwapBuffers();
			}
			return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
		}

		[DllImport("glfw3.dll")]
		private static extern int glfwInit();

		[DllImport("glfw3.dll")]
		private static extern void glfwWindowHint(int hint, int value);

		[DllImport("glfw3.dll")]
		private static extern void glfwTerminate();

		[DllImport("glfw3.dll")]
		private static extern void glfwPollEvents();

		[DllImport("glfw3.dll")]
		private static extern IntPtr glfwCreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);

		[DllImport("glfw3.dll")]
		private static extern void glfwMakeContextCurrent(IntPtr window);

		[DllImport("glfw3.dll")]
		private static extern void glfwSwapBuffers(IntPtr window);

		[DllImport("glfw3.dll")]
		private static extern void glfwDestroyWindow(IntPtr window);

		[DllImport("glfw3.dll")]
		private static extern IntPtr glfwGetWin32Window(IntPtr window);

		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr child, IntPtr parent);

		[DllImport("user32.dll")]
		private static extern UInt32 SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);
	}
}

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.Internal.VisualStudio.PlatformUI;
using WindowsUtilities;

namespace Forces.Windows
{
	public class RenderWindow : HwndHost
	{
		private readonly int _multisampling;
		private IntPtr _windowHandle;

		public event EventHandler ContextInitialized;
		public event EventHandler Paint;
		public new event EventHandler<Point> MouseMove;
		public new event EventHandler<Point> MouseRightButtonDown;
		public new event EventHandler<Point> MouseRightButtonUp;

		public RenderWindow(int multisampling)
		{
			_multisampling = multisampling;
		}

		public bool EnableRawCursor
		{
			set
			{
				if (glfwRawMouseMotionSupported() != 0)
				{
					glfwSetInputMode(_windowHandle, 0x00033005, value ? 1 : 0);
				}
			}
		}

		public bool DisableCursor
		{
			set => glfwSetInputMode(_windowHandle, 0x00033001, value ? 0x00034003 : 0x00034001);
		}

		public void MakeContextCurrent()
		{
			glfwMakeContextCurrent(_windowHandle);
		}

		public void SwapBuffers()
		{
			glfwSwapBuffers(_windowHandle);
		}

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			glfwInit();
			glfwWindowHint(0x0002100D, _multisampling);
			_windowHandle = glfwCreateWindow(100, 100, "Forces Preview", IntPtr.Zero, IntPtr.Zero);
			var win32Handle = glfwGetWin32Window(_windowHandle);
			const int GWL_STYLE = (-16);
			const int WS_CHILD = 0x40000000;
			SetWindowLong(win32Handle, GWL_STYLE, WS_CHILD);
			SetParent(win32Handle, hwndParent.Handle);
			MakeContextCurrent();
			ContextInitialized?.Invoke(this, null);
			return new HandleRef(this, win32Handle);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			if (!ReferenceEquals(hwnd.Wrapper, this)) return;
			glfwDestroyWindow(_windowHandle);
			_windowHandle = IntPtr.Zero;
			glfwTerminate();
		}

		protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch (msg)
			{
				case (int)WindowsMessages.WM_NCHITTEST:
				{
					handled = true;
					return new IntPtr(1);
				}
				case (int)WindowsMessages.WM_MOUSEMOVE:
				{
					handled = true;
					var xPos = lParam.ToInt32() & 0xFFFF; 
					var yPos = (lParam.ToInt32() >> 16) & 0xFFFF;
					MouseMove?.Invoke(this, new Point(xPos, yPos));
					break;
				}
				case (int)WindowsMessages.WM_RBUTTONDOWN:
				{
					handled = true;
					var xPos = lParam.ToInt32() & 0xFFFF; 
					var yPos = (lParam.ToInt32() >> 16) & 0xFFFF;
					MouseRightButtonDown?.Invoke(this, new Point(xPos, yPos));
					break;
				}
				case (int)WindowsMessages.WM_RBUTTONUP:
				{
					handled = true;
					var xPos = lParam.ToInt32() & 0xFFFF; 
					var yPos = (lParam.ToInt32() >> 16) & 0xFFFF;
					MouseRightButtonUp?.Invoke(this, new Point(xPos, yPos));
					break;
				}
				case (int)WindowsMessages.WM_PAINT:
				{
					Paint?.Invoke(this, null);
					break;
				}
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
		private static extern int glfwRawMouseMotionSupported();

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

		[DllImport("glfw3.dll")]
		private static extern void glfwSetInputMode(IntPtr window, int mode, int value);

		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr child, IntPtr parent);

		[DllImport("user32.dll")]
		private static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
	}
}

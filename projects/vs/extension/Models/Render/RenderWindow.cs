using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.VisualStudio.Utilities;
using WindowsUtilities;

namespace Forces.Models.Render
{
	public class RenderWindow : HwndHost
	{
		private readonly int _multisampling;
		public IntPtr WindowHandle { get; private set; }
		private int _lastX = 0;
		private int _lastY = 0;

		public event EventHandler ContextInitialized;
		public event EventHandler Paint;

		public event EventHandler<Point> RawMouseMove;
		public new event EventHandler<Point> MouseMove;
		public new event EventHandler<Point> MouseRightButtonDown;
		public new event EventHandler<Point> MouseRightButtonUp;

		public RenderWindow(int multisampling)
		{
			_multisampling = multisampling;
		}

		public System.Drawing.Size RenderScaledSize =>
			new System.Drawing.Size()
			{
				Width = (int)(RenderSize.Width * this.GetDpiXScale()),
				Height = (int)(RenderSize.Height * this.GetDpiYScale())
			};

		public bool EnableRawCursor
		{
			set
			{
				RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];
				rid[0].usUsagePage = Convert.ToUInt16(1);
				rid[0].usUsage = Convert.ToUInt16(2);
				rid[0].dwFlags = value ? 0 : 1;
				rid[0].hwndTarget = value ? Handle : IntPtr.Zero;
				if (!RegisterRawInputDevices(rid, Convert.ToUInt32(rid.Length),
						Convert.ToUInt32(Marshal.SizeOf(rid[0]))))
				{
					throw new ApplicationException("Failed to register raw input device(s). " +
												   "Error code: " + Marshal.GetLastWin32Error());
				}
			}
		}

		public bool DisableCursor
		{
			set
			{
				glfwSetInputMode(WindowHandle, 0x00033001, value ? 0x00034003 : 0x00034001);
				if (value)
				{
					var origin = PointToScreen(new Point(0, 0));
					var rect = new System.Drawing.Rectangle((int)origin.X, (int)origin.Y, (int)(origin.X + RenderSize.Width * this.GetDpiXScale()), (int)(origin.Y + RenderSize.Height * this.GetDpiYScale()));
					ClipCursor(ref rect);
				}
				else
				{
					ClipCursor(IntPtr.Zero);
				}
			}
		}

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			glfwInit();
			glfwWindowHint(0x0002100D, _multisampling);
			WindowHandle = glfwCreateWindow(100, 100, "Forces Preview", IntPtr.Zero, IntPtr.Zero);
			var win32Handle = glfwGetWin32Window(WindowHandle);
			const int GWL_STYLE = (-16);
			const int WS_CHILD = 0x40000000;
			SetWindowLong(win32Handle, GWL_STYLE, WS_CHILD);
			SetParent(win32Handle, hwndParent.Handle);
			glfwMakeContextCurrent(WindowHandle);
			ContextInitialized?.Invoke(this, null);
			return new HandleRef(this, win32Handle);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			if (!ReferenceEquals(hwnd.Wrapper, this)) return;
			glfwDestroyWindow(WindowHandle);
			WindowHandle = IntPtr.Zero;
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
				case (int)WindowsMessages.WM_INPUT:
				{
					handled = true;
					uint size = 0;
					if (GetRawInputData(lParam, 0x10000003, IntPtr.Zero, ref size,
						    Marshal.SizeOf<RAWINPUTHEADER>()) != 0)
					{
						throw new ApplicationException("Failed to get raw input. " +
						                               "Error code: " + Marshal.GetLastWin32Error());
					}
					var count = size/Marshal.SizeOf<RAWINPUT>();
					var input = new RAWINPUT();
					if (GetRawInputData(lParam, 0x10000003, ref input, ref size,
						    Marshal.SizeOf<RAWINPUTHEADER>()) == 0xFFFFFFFF)
					{
						throw new ApplicationException("Failed to get raw input. " +
						                               "Error code: " + Marshal.GetLastWin32Error());
					}
					if (input.header.dwType == 0) //RIM_TYPEMOUSE
					{
						if ((input.data.Mouse.usFlags & 0x01) != 0) //MOUSE_MOVE_ABSOLUTE
						{
							var dx = input.data.Mouse.lLastX - _lastX;
							_lastX = input.data.Mouse.lLastX;
							var dy = input.data.Mouse.lLastY - _lastY;
							_lastY = input.data.Mouse.lLastY;
							RawMouseMove?.Invoke(this, new Point(dx, dy));
						}
						else
						{
							RawMouseMove?.Invoke(this, new Point(input.data.Mouse.lLastX, input.data.Mouse.lLastY));
						}
					}
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

		public HashSet<Key> PressedKeys()
		{
			byte[] keyStates = new byte[256];
			if (!GetKeyboardState(keyStates))
				throw new Win32Exception(Marshal.GetLastWin32Error());
			return keyStates
				.Select((b, i) => ((b & 0x80) != 0, i))
				.Where(x=>x.Item1)
				.Select(x=>KeyInterop.KeyFromVirtualKey(x.i))
				.ToHashSet();
		}

		[DllImport("glfw3.dll")]
		private static extern int glfwInit();

		[DllImport("glfw3.dll")]
		private static extern void glfwWindowHint(int hint, int value);

		[DllImport("glfw3.dll")]
		private static extern void glfwTerminate();

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

		[DllImport("user32.dll")]
		private static extern bool GetKeyboardState(byte[] states);

		[DllImport("user32.dll")]
		private static extern void ClipCursor(ref System.Drawing.Rectangle rect);

		[DllImport("user32.dll")]
		private static extern void ClipCursor(IntPtr rect);

		[DllImport("User32.dll", SetLastError = true)]
		private static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, UInt32 uiNumDevices, UInt32 cbSize);

		[StructLayout(LayoutKind.Sequential)]
		private struct RAWINPUTDEVICE
		{
			[MarshalAs(UnmanagedType.U2)]
			public ushort usUsagePage;
			[MarshalAs(UnmanagedType.U2)]
			public ushort usUsage;
			[MarshalAs(UnmanagedType.U4)]
			public int dwFlags;
			public IntPtr hwndTarget;
		}

		[DllImport("User32.dll", SetLastError = true)]
		private static extern uint GetRawInputData(IntPtr hRawInput, uint uiCommand, ref RAWINPUT pData, ref uint pcbSize, int cbSizeHeader);

		[DllImport("User32.dll", SetLastError = true)]
		private static extern uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr nullable, ref uint pcbSize, int cbSizeHeader);

		[StructLayout(LayoutKind.Sequential)]
		private struct RAWINPUT
		{
			public RAWINPUTHEADER header;
			public RAWINPUTDATA data;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct RAWINPUTHEADER
		{
			public int dwType;
			public int dwSize;
			public IntPtr hDevice;
			public IntPtr wParam;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct RAWMOUSE
		{
			[MarshalAs(UnmanagedType.U2)]
			public ushort usFlags;
			[MarshalAs(UnmanagedType.U2)]
			public ushort usButtonFlags;
			[MarshalAs(UnmanagedType.U2)]
			public ushort usButtonData;
			[MarshalAs(UnmanagedType.U4)]
			public uint ulRawButtons;
			[MarshalAs(UnmanagedType.I4)]
			public int lLastX;
			[MarshalAs(UnmanagedType.I4)]
			public int lLastY;
			[MarshalAs(UnmanagedType.U4)]
			public uint ulExtraInformation;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct RAWINPUTDATA
		{
			[FieldOffset(0)]
			public RAWMOUSE Mouse;
			[FieldOffset(0)]
			public RAWKEYBOARD Keyboard;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct RAWKEYBOARD
		{
			public ushort MakeCode;
			public ushort Flags;
			public ushort Reserved;
			public ushort VKey;
			public uint Message;
			public ulong ExtraInformation;
		}
	}
}

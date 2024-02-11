using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Forces.Models.SceneTree;
using Forces.Utilities;
using Forces.Windows;

namespace Forces.Controllers
{
	public class PreviewCameraMovementController : IDisposable
	{
		private readonly RenderWindow _window;
		private readonly PreviewCamera _camera;
		private bool _mouseRightPressed;
		private Point _lastCursorPos;
		private readonly DispatcherTimer _controlsTimer;
		private readonly HashSet<Key> _pressedKeys = new HashSet<Key>();

		public PreviewCameraMovementController(RenderWindow window, PreviewCamera camera)
		{
			_window = window;
			_camera = camera;
			_window.MouseRightButtonUp += Window_MouseRightButtonUp;
			_window.MouseRightButtonDown += Window_MouseRightButtonDown;
			_window.MouseMove += Window_MouseMove;
			_controlsTimer = new DispatcherTimer(DispatcherPriority.Normal)
			{
				Interval = new TimeSpan(0, 0, 0, 0, 5)
			};
			_controlsTimer.Tick += _controlsTimer_Tick;
		}

		private void _controlsTimer_Tick(object sender, EventArgs e)
		{
			float moveSpeed = 0.2f;
			var pressedKeys = _window.PressedKeys();
			if (pressedKeys.Contains(Key.W))
			{
				_camera.Translation += moveSpeed * Front(_camera);
			}
			if (pressedKeys.Contains(Key.S))
			{
				_camera.Translation -= moveSpeed * Front(_camera);
			}
			if (pressedKeys.Contains(Key.A))
			{
				_camera.Translation += moveSpeed * Right(_camera);
			}
			if (pressedKeys.Contains(Key.D))
			{
				_camera.Translation -= moveSpeed * Right(_camera);
			}
		}

		public void Dispose()
		{
			_window.MouseRightButtonDown -= Window_MouseRightButtonDown;
			_window.MouseRightButtonUp -= Window_MouseRightButtonUp;
			_window.MouseMove -= Window_MouseMove;
			_controlsTimer.Stop();
		}

		private void Window_MouseMove(object sender, Point e)
		{
			if (_mouseRightPressed)
			{
				var cursorPos = e;
				var sens = 0.2f;
				var cursorMovement = cursorPos - _lastCursorPos;
				Pitch(_camera, (float)(cursorMovement.Y * sens));
				Yaw(_camera, (float)(-cursorMovement.X * sens));
				_lastCursorPos = cursorPos;
			}
		}

		private void Window_MouseRightButtonDown(object sender, Point e)
		{
			_window.Focus();
			var cursorPos = e;
			if (!_mouseRightPressed)
			{
				_window.DisableCursor = true;
				_window.EnableRawCursor = true;
				_lastCursorPos = cursorPos;
				_mouseRightPressed = true;
				_controlsTimer.Start();
			}
		}

		private void Window_MouseRightButtonUp(object sender, Point e)
		{
			if (_mouseRightPressed)
			{
				_window.EnableRawCursor = false;
				_window.DisableCursor = false;
				_mouseRightPressed = false;
				_controlsTimer.Stop();
			}
		}

		static void Yaw(PreviewCamera camera, float grad)
		{
			camera.Rotation *= new Vector3(0.0f, grad.ToRadians(), 0.0f ).EulerXYZToQuaternion();
		}

		static void Pitch(PreviewCamera camera, float grad)
		{
			camera.Rotation *= new Vector3(grad.ToRadians(), 0.0f, 0.0f).EulerXYZToQuaternion();
		}

		static void Roll(PreviewCamera camera, float grad)
		{
			camera.Rotation *= new Vector3(0.0f, 0.0f, grad.ToRadians()).EulerXYZToQuaternion();
		}

		static Vector3 Front(PreviewCamera camera)
		{
			return camera.Rotation.Multiply(new Vector3(0.0f, 0.0f, 1.0f));
		}

		static Vector3 Right(PreviewCamera camera)
		{
			return camera.Rotation.Multiply(new Vector3(1.0f, 0.0f, 0.0f));
		}
	}
}
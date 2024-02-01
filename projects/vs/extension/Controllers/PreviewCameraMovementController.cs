using System;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using Forces.Models.SceneTree;
using Forces.Utilities;
using Forces.Windows;

namespace Forces.Controllers
{
	public class PreviewCameraMovementController : IDisposable
	{
		private readonly RenderWindow _window;
		private readonly PreviewCamera _camera;
		private bool _mouseMiddlePressed;
		private Point _lastCursorPos;

		public PreviewCameraMovementController(RenderWindow window, PreviewCamera camera)
		{
			_window = window;
			_camera = camera;
			_window.MouseDown += Window_MouseDown;
			_window.MouseUp += Window_MouseUp;
			_window.MouseMove += Window_MouseMove;
			_window.KeyDown += Window_KeyDown;
		}

		public void Dispose()
		{
			_window.MouseDown -= Window_MouseDown;
			_window.MouseUp -= Window_MouseUp;
			_window.MouseMove -= Window_MouseMove;
			_window.KeyDown -= Window_KeyDown;
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{

		}
		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			if (_mouseMiddlePressed)
			{
				var cursorPos = e.GetPosition(_window);
				var sens = 0.2f;
				var cursorMovement = cursorPos - _lastCursorPos;
				Pitch(_camera, (float)(cursorMovement.Y * sens));
				Yaw(_camera, (float)(-cursorMovement.X * sens));
				_lastCursorPos = cursorPos;
			}
		}

		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (e.MiddleButton == MouseButtonState.Released)
			{
				if (_mouseMiddlePressed)
				{
					_window.EnableRawCursor = false;
					_window.DisableCursor = false;
					_mouseMiddlePressed = false;
				}
			}
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				var cursorPos = e.GetPosition(_window);
				if (!_mouseMiddlePressed)
				{
					_window.DisableCursor = true;
					_window.EnableRawCursor = true;
					_lastCursorPos = cursorPos;
					_mouseMiddlePressed = true;
				}
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
	}
}
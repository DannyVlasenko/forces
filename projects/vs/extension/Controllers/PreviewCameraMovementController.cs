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
			_window.MouseRightButtonUp += _window_MouseRightButtonUp;
			_window.MouseRightButtonDown += Window_MouseRightButtonDown;
			_window.MouseMove += Window_MouseMove;
			_window.KeyDown += Window_KeyDown;
		}

		public void Dispose()
		{
			_window.MouseRightButtonDown -= Window_MouseRightButtonDown;
			_window.MouseRightButtonUp -= _window_MouseRightButtonUp;
			_window.MouseMove -= Window_MouseMove;
			_window.KeyDown -= Window_KeyDown;
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{

		}
		private void Window_MouseMove(object sender, Point e)
		{
			if (_mouseMiddlePressed)
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
			if (!_mouseMiddlePressed)
			{
				_window.DisableCursor = true;
				_window.EnableRawCursor = true;
				_lastCursorPos = cursorPos;
				_mouseMiddlePressed = true;
			}
		}

		private void _window_MouseRightButtonUp(object sender, Point e)
		{
			if (_mouseMiddlePressed)
			{
				_window.EnableRawCursor = false;
				_window.DisableCursor = false;
				_mouseMiddlePressed = false;
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
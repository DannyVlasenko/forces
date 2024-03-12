using System;
using Forces.Models.Render;
using Forces.Models.SceneTree;
using System.Windows;

namespace Forces.Controllers
{
	public class PreviewCameraViewportController : IDisposable
	{
		private readonly RenderWindow _window;
		private readonly Camera _camera;

		public PreviewCameraViewportController(RenderWindow window, Camera camera)
		{
			_window = window;
			_camera = camera;
			_window.SizeChanged += _window_SizeChanged;
			_window.Loaded += _window_Loaded;
		}

		private void _window_Loaded(object sender, RoutedEventArgs e)
		{
			_camera.Viewport = _window.RenderScaledSize;
		}

		private void _window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			_camera.Viewport = _window.RenderScaledSize;
		}

		public void Dispose()
		{
			_window.SizeChanged -= _window_SizeChanged;
			_window.Loaded -= _window_Loaded;
		}
	}
}
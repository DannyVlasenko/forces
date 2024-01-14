using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using Forces.Engine;
using Forces.Models;
using Forces.ViewModels;
using Microsoft.VisualStudio.Utilities;

namespace Forces.Windows
{
	[Guid("5e8a8814-5f59-48fd-ade2-911513fcc6e2")]
	public sealed class PreviewWindow : ToolWindowPane
	{
		private readonly Window _window;
		private readonly PreviewWindowViewModel _viewModel;
		private OpenGLRenderer _renderer;

		public PreviewWindow(PreviewWindowViewModel viewModel) : base(null)
		{
			_window = new Window(this);
			_window.ContextInitialized += _window_ContextInitialized;
			_window.Paint += _window_Paint;
			_window.SizeChanged += _window_SizeChanged;
			_window.Loaded += _window_Loaded;
			Content = _window;

			_viewModel = viewModel;
			_viewModel.RenderRootNodeChanged += _viewModel_RenderRootNodeChanged;
			_viewModel.CameraChanged += _viewModel_CameraChanged;
			Caption = _viewModel.WindowTitle;
			_viewModel.WindowTitleChanged += (sender, title) => Caption = title;
		}

		private void _viewModel_RenderRootNodeChanged(object sender, Node rootNode)
		{
			Caption = _viewModel?.WindowTitle;
			if (rootNode == null) return;
			_window.MakeContextCurrent();
			_renderer.SetCurrentRootNode(rootNode);
			_renderer?.Render();
			_window.SwapBuffers();
		}

		private void _viewModel_CameraChanged(object sender, Camera camera)
		{
			if (camera == null) return;
			_window.MakeContextCurrent();
			_renderer.SetCamera(camera);
			_renderer?.Render();
			_window.SwapBuffers();
		}

		private void _window_ContextInitialized(object sender, System.EventArgs e)
		{
			_renderer = new OpenGLRenderer();
			if (_viewModel?.RenderRootNode != null)
			{
				_renderer.SetCurrentRootNode(_viewModel?.RenderRootNode);
			}
			if (_viewModel?.Camera != null)
			{
				_renderer.SetCamera(_viewModel.Camera);
			}
		}

		private void _window_Paint(object sender, System.EventArgs e)
		{
			_window.MakeContextCurrent();
			_renderer?.Render();
			_window.SwapBuffers();
		}
		
		private void _window_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			SetCameraViewport();
		}
		private void _window_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			SetCameraViewport();
		}

		private void SetCameraViewport()
		{
			if (_viewModel.Camera == null)
			{
				return;
			}

			_viewModel.Camera.Viewport = new Vec2()
			{
				X = (float)(_window.RenderSize.Width * _window.GetDpiXScale()),
				Y = (float)(_window.RenderSize.Height * _window.GetDpiYScale())
			};
		}
	}
}

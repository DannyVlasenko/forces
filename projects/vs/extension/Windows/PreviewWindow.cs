using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using Forces.Engine;
using Forces.ViewModels;
using Microsoft.VisualStudio.Utilities;

namespace Forces.Windows
{
	[Guid("5e8a8814-5f59-48fd-ade2-911513fcc6e2")]
	public sealed class PreviewWindow : ToolWindowPane
	{
		private readonly RenderWindow _renderWindow;
		private readonly PreviewWindowViewModel _viewModel;
		private OpenGLRenderer _renderer;

		public PreviewWindow(PreviewWindowViewModel viewModel) : base(null)
		{
			_renderWindow = new RenderWindow(this);
			_renderWindow.ContextInitialized += RenderWindowContextInitialized;
			_renderWindow.Paint += RenderWindowPaint;
			_renderWindow.SizeChanged += RenderWindowSizeChanged;
			_renderWindow.Loaded += RenderWindowLoaded;
			Content = _renderWindow;

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
			_renderWindow.MakeContextCurrent();
			_renderer.SetCurrentRootNode(rootNode);
			_renderer?.Render();
			_renderWindow.SwapBuffers();
		}

		private void _viewModel_CameraChanged(object sender, Camera camera)
		{
			if (camera == null) return;
			_renderWindow.MakeContextCurrent();
			_renderer.SetCamera(camera);
			_renderer?.Render();
			_renderWindow.SwapBuffers();
		}

		private void RenderWindowContextInitialized(object sender, System.EventArgs e)
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

		private void RenderWindowPaint(object sender, System.EventArgs e)
		{
			_renderWindow.MakeContextCurrent();
			_renderer?.Render();
			_renderWindow.SwapBuffers();
		}
		
		private void RenderWindowSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			SetCameraViewport();
		}
		private void RenderWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
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
				X = (float)(_renderWindow.RenderSize.Width * _renderWindow.GetDpiXScale()),
				Y = (float)(_renderWindow.RenderSize.Height * _renderWindow.GetDpiYScale())
			};
		}
	}
}

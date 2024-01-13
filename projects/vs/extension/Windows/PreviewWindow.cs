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
		private readonly SelectionModel _selectionModel;
		private readonly Window _window;
		private OpenGLRenderer _renderer;

		private string Title => "Forces Preview - " + ((_selectionModel?.SceneName?.Length ?? 0) == 0 ? "No Scene Selected" : _selectionModel.SceneName);

		public PreviewWindow(SelectionModel selectionModel) : base(null)
		{
			_selectionModel = selectionModel;
			_window = new Window(this);
			_window.ContextInitialized += _window_ContextInitialized;
			_window.Paint += _window_Paint;
			Content = _window;
			Caption = Title;
			_selectionModel.SelectedSceneChanged += SelectionModel_SceneChanged;
			_selectionModel.CameraChanged += SelectionModel_CameraChanged;
		}

		private void SelectionModel_CameraChanged(object sender, Camera e)
		{
			if (e == null) return;
			_window.MakeContextCurrent();
			_renderer.SetCamera(e);
			_renderer?.Render();
			_window.SwapBuffers();
		}

		private void SelectionModel_SceneChanged(object sender, Scene e)
		{
			Caption = Title;
			if (e?.RootNode == null) return;
			_window.MakeContextCurrent();
			_renderer.SetCurrentRootNode(e.RootNode);
			_renderer?.Render();
			_window.SwapBuffers();
		}

		private void _window_ContextInitialized(object sender, System.EventArgs e)
		{
			_renderer = new OpenGLRenderer();
			if (_selectionModel?.SelectedScene?.RootNode != null)
			{
				_renderer.SetCurrentRootNode(_selectionModel.SelectedScene.RootNode);
			}
			if (_selectionModel?.SelectedScene?.PreviewCamera != null)
			{
				_renderer.SetCamera(_selectionModel.SelectedScene.PreviewCamera);
			}
		}

		private void _window_Paint(object sender, System.EventArgs e)
		{
			_window.MakeContextCurrent();
			if (_selectionModel?.SelectedScene?.PreviewCamera != null)
			{

				_selectionModel.SelectedScene.PreviewCamera.Viewport = new Vec2()
				{
					X = (float)(_window.RenderSize.Width * _window.GetDpiXScale()),
					Y = (float)(_window.RenderSize.Height * _window.GetDpiYScale())
				};
			}
			_renderer?.Render();
			_window.SwapBuffers();
		}
	}
}

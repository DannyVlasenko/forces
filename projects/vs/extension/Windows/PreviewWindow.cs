using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using Forces.Engine;
using Forces.Models;
using Forces.ViewModels;
using Microsoft.VisualStudio.Utilities;

namespace Forces.Windows
{
	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	/// </summary>
	/// <remarks>
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
	/// usually implemented by the package implementer.
	/// <para>
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its
	/// implementation of the IVsUIElementPane interface.
	/// </para>
	/// </remarks>
	[Guid("5e8a8814-5f59-48fd-ade2-911513fcc6e2")]
	public class PreviewWindow : ToolWindowPane
	{
		private readonly SelectedSceneModel _sceneModel;
		private readonly Window _window;
		private OpenGLRenderer _renderer;

		private string Title => "Forces Preview - " + ((_sceneModel?.SceneName?.Length ?? 0) == 0 ? "No Scene Selected" : _sceneModel.SceneName);

		public PreviewWindow(SelectedSceneModel sceneModel) : base(null)
		{
			_sceneModel = sceneModel;
			_window = new Window(this);
			_window.ContextInitialized += _window_ContextInitialized;
			_window.Paint += _window_Paint;
			this.Content = _window;
			this.Caption = Title;
			_sceneModel.SelectedSceneChanged += _sceneModel_SelectedSceneChanged;
			_sceneModel.CameraChanged += _sceneModel_CameraChanged;
		}

		private void _sceneModel_CameraChanged(object sender, Camera e)
		{
			if (e != null)
			{
				_window.MakeContextCurrent();
				_renderer.SetCamera(_sceneModel.SelectedScene.PreviewCamera);
				_renderer?.Render();
				_window.SwapBuffers();
			}
		}

		private void _sceneModel_SelectedSceneChanged(object sender, Scene e)
		{
			this.Caption = Title;
			if (e?.RootNode != null && e.PreviewCamera != null)
			{
				_window.MakeContextCurrent();
				_renderer.SetCurrentRootNode(e.RootNode);
				_renderer.SetCamera(_sceneModel.SelectedScene.PreviewCamera);
				_renderer?.Render();
				_window.SwapBuffers();
			}
		}

		private void _window_ContextInitialized(object sender, System.EventArgs e)
		{
			_renderer = new OpenGLRenderer();
			if (_sceneModel?.SelectedScene?.RootNode != null)
			{
				_renderer.SetCurrentRootNode(_sceneModel.SelectedScene.RootNode);
			}
			if (_sceneModel?.SelectedScene?.PreviewCamera != null)
			{
				_renderer.SetCamera(_sceneModel.SelectedScene.PreviewCamera);
			}
		}

		private void _window_Paint(object sender, System.EventArgs e)
		{
			_window.MakeContextCurrent();
			if (_sceneModel?.SelectedScene?.PreviewCamera != null)
			{

				_sceneModel.SelectedScene.PreviewCamera.Viewport = new Vec2()
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

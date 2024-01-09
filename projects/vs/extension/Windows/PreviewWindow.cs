using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using Forces.Engine;
using Forces.Models;
using Forces.ViewModels;

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

		public PreviewWindow(SelectedSceneModel sceneModel) : base(null)
		{
			_sceneModel = sceneModel;
			this.Caption = "Forces Preview";
			_window = new Window(this);
			_window.ContextInitialized += _window_ContextInitialized;
			_window.Paint += _window_Paint;
			this.Content = _window;
			_sceneModel.SelectedSceneChanged += _sceneModel_SelectedSceneChanged;
		}

		private void _sceneModel_SelectedSceneChanged(object sender, Scene e)
		{
			_renderer.SetCurrentRootNode(e.RootNode);
		}

		private void _window_ContextInitialized(object sender, System.EventArgs e)
		{
			_renderer = new OpenGLRenderer();
			if (_sceneModel?.SelectedScene?.RootNode != null)
			{
				_renderer.SetCurrentRootNode(_sceneModel.SelectedScene.RootNode);
			}
		}

		private void _window_Paint(object sender, System.EventArgs e)
		{
			_window.MakeContextCurrent();
			_renderer?.Render();
			_window.SwapBuffers();
		}
	}
}

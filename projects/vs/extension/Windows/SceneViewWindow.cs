using System.Runtime.InteropServices;
using Forces.Engine;
using Forces.Models;
using Forces.ViewModels;
using Microsoft.VisualStudio.Shell;

namespace Forces.Windows
{
	[Guid("606da3ec-a27c-4e36-b3fd-4d13f23204df")]
	public sealed class SceneViewWindow : ToolWindowPane
	{
		private readonly SelectionModel _model;
		private readonly SceneViewWindowControl _control;
		private Scene _currentScene;

		private string Title => "Forces Scene - " + ((_model?.SceneName?.Length ?? 0) == 0 ? "No Scene Selected" : _model.SceneName);
		public SceneViewWindow(SelectionModel model) : base(null)
		{
			_model = model;
			_control = new SceneViewWindowControl();
			_control.SelectedItemChanged += _control_SelectedItemChanged;
			Content = _control;
			Caption = Title;

			if (_model.SelectedScene?.RootNode != null)
			{
				_currentScene = _model.SelectedScene;
				_control.AddItem(new SceneViewNode(_model.SelectedScene.RootNode));
				_control.AddItem(new SceneViewCamera(_model.SelectedScene.PreviewCamera));
				model.SelectSceneViewNode(_control.SelectedItem);
			}

			_model.SelectedSceneChanged += _model_SelectedSceneChanged;
		}

		private void _control_SelectedItemChanged(object sender, ISceneViewNode e)
		{
			_model.SelectSceneViewNode(_control.SelectedItem);
		}

		private void _model_SelectedSceneChanged(object sender, Engine.Scene e)
		{
			if (_currentScene == e)
			{
				return;
			}
			Caption = Title;
			_control.ClearItems();
			if (_model.SelectedScene?.RootNode != null)
			{
				_currentScene = _model.SelectedScene;
				_control.AddItem(new SceneViewNode(_model.SelectedScene.RootNode));
				_control.AddItem(new SceneViewCamera(_model.SelectedScene.PreviewCamera));
				_model.SelectSceneViewNode(_control.SelectedItem);
			}
		}
	}
}

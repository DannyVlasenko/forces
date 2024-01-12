using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Forces.Engine;
using Forces.Models;
using Forces.ViewModels;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Forces.Windows
{
	[Guid("606da3ec-a27c-4e36-b3fd-4d13f23204df")]
	public sealed class SceneViewWindow : ToolWindowPane
	{
		private readonly SelectedSceneModel _model;
		private IVsWindowFrame _frame = null;
		private readonly SceneViewWindowControl _control;
		private Scene _currentScene;

		private string Title => "Forces Scene - " + ((_model?.SceneName?.Length ?? 0) == 0 ? "No Scene Selected" : _model.SceneName);
		public SceneViewWindow(SelectedSceneModel model) : base(null)
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
				TrackSelection(_control.SelectedItem);
			}

			_model.SelectedSceneChanged += _model_SelectedSceneChanged;
		}

		private void _control_SelectedItemChanged(object sender, ISceneViewNode e)
		{
			if (e is SceneViewNode svn)
			{
				TrackSelection(new NodePropertiesModel(svn.Node, _model));
			}
			if (e is SceneViewCamera svc)
			{
				TrackSelection(new CameraPropertiesModel(svc.Camera, _model));
			}
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
				TrackSelection(_control.SelectedItem);
			}
		}

		private void TrackSelection(object selection)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			if (_frame == null)
			{
				if (GetService(typeof(SVsUIShell)) is IVsUIShell shell)
				{
					var guidPropertyBrowser = new Guid(ToolWindowGuids.PropertyBrowser);
					shell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guidPropertyBrowser, out _frame);
				}
			}

			_frame?.Show();
			var mySelContainer = new SelectionContainer()
			{
				SelectedObjects = new System.Collections.ArrayList()
				{
					selection
				}
			};

			if (GetService(typeof(STrackSelection)) is ITrackSelection track)
			{
				track.OnSelectChange(mySelContainer);
			}
		}
	}
}

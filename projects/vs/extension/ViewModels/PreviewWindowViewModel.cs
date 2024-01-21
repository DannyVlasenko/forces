using System;
using Forces.Models;
using Forces.Models.SceneTree;

namespace Forces.ViewModels
{
	public class PreviewWindowViewModel
	{
		private readonly SelectionModel _selectionModel;
		public Camera Camera { get; private set; }
		public event EventHandler<Camera> CameraChanged;

		private Scene _selectedScene;
		public Node RenderRootNode => _selectedScene.RootNode;
		public event EventHandler<Node> RenderRootNodeChanged;

		public string WindowTitle => "Forces Preview - " + (string.IsNullOrEmpty(_selectionModel.SceneName) ? "No Scene Selected" : _selectionModel.SceneName);
		public event EventHandler<string> WindowTitleChanged;

		public PreviewWindowViewModel(SelectionModel selectionModel)
		{
			_selectionModel = selectionModel;
			_selectionModel.SelectedCameraChanged += _selectionModel_SelectedCameraChanged;
			_selectionModel.SelectedSceneChanged += _selectionModel_SelectedSceneChanged;
			Camera = _selectionModel.SelectedCamera ?? new Camera();
			Camera.PropertyChanged += Camera_PropertyChanged;
			_selectedScene = _selectionModel.SelectedScene ?? new Scene();
			_selectedScene.NodeChanged += _selectedScene_NodeChanged;
		}

		private void Camera_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			CameraChanged?.Invoke(this, Camera);
		}

		private void _selectionModel_SelectedCameraChanged(object sender, Camera e)
		{
			Camera.PropertyChanged -= Camera_PropertyChanged;
			Camera = _selectionModel.SelectedCamera;
			Camera.PropertyChanged += Camera_PropertyChanged;
			CameraChanged?.Invoke(this, Camera);
		}

		private void _selectedScene_NodeChanged(object sender, Node e)
		{
			RenderRootNodeChanged?.Invoke(this, RenderRootNode);
		}

		private void _selectionModel_SelectedSceneChanged(object sender, Scene scene)
		{
			_selectedScene.NodeChanged -= _selectedScene_NodeChanged;
			_selectedScene = scene;
			_selectedScene.NodeChanged += _selectedScene_NodeChanged;
			RenderRootNodeChanged?.Invoke(this, RenderRootNode);
			WindowTitleChanged?.Invoke(this, WindowTitle);
		}
	}
}

using System;
using System.IO;
using Forces.Engine;
using Forces.ViewModels;

namespace Forces.Models
{
	public class SelectionModel
	{
		public ISceneViewNode SelectedSceneViewNode { get; private set; }
		public Scene SelectedScene { get; private set; }
		public Camera SelectedCamera { get; private set; }
		public string SceneName { get; private set; }

		public event EventHandler<Scene> SelectedSceneChanged;
		public event EventHandler<Camera> SelectedCameraChanged;
		public event EventHandler<ISceneViewNode> SelectedSceneViewNodeChanged;

		public void SelectScene(string filePath)
		{
			SelectedScene = new Scene();
			SceneName = Path.GetFileNameWithoutExtension(filePath);
			SelectedCamera = SelectedScene.PreviewCamera;
			SelectedSceneChanged?.Invoke(this, SelectedScene);
			SelectedCameraChanged?.Invoke(this, SelectedScene.PreviewCamera);
		}

		public void SelectCamera(Camera camera)
		{
			SelectedCamera = camera;
			SelectedCameraChanged?.Invoke(this, camera);
		}

		public void SelectSceneViewNode(ISceneViewNode selectedNode)
		{
			if (ReferenceEquals(SelectedSceneViewNode, selectedNode)) return;
			SelectedSceneViewNode = selectedNode;
			SelectedSceneViewNodeChanged?.Invoke(this, SelectedSceneViewNode);
		}
	}
}

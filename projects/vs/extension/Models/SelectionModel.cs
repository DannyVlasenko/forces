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
		public string SceneName { get; private set; }

		public event EventHandler<Scene> SelectedSceneChanged;
		public event EventHandler<Camera> CameraChanged;
		public event EventHandler<ISceneViewNode> SelectedSceneViewNodeChanged;

		public void SelectScene(string filePath)
		{
			SelectedScene = new Scene();
			SceneName = Path.GetFileNameWithoutExtension(filePath);
			SelectedSceneChanged?.Invoke(this, SelectedScene);
			CameraChanged?.Invoke(this, SelectedScene.PreviewCamera);
		}

		public void SelectCamera(Camera camera)
		{
			SelectedScene.PreviewCamera = camera;
			CameraChanged?.Invoke(this, camera);
		}

		public void SelectSceneViewNode(ISceneViewNode selectedNode)
		{
			SelectedSceneViewNode = selectedNode;
			SelectedSceneViewNodeChanged?.Invoke(this, SelectedSceneViewNode);
		}
	}
}

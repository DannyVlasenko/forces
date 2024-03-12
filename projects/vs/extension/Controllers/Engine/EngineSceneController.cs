using System;
using Forces.Models;
using Forces.Models.Render;
using EditorScene = Forces.Models.SceneTree.Scene;
using EngineScene = Forces.Models.Engine.Scene;

namespace Forces.Controllers.Engine
{
	public class EngineSceneController : IDisposable
	{
		private readonly EngineScene _engineScene;
		private readonly EngineEmptyNodeController _rootNodeController;
		private readonly MeshModel _meshModel = new MeshModel();

		public EngineSceneController(EditorScene editorScene, OpenGLRenderer renderer)
		{
			_engineScene = new EngineScene();
			_rootNodeController = new EngineEmptyNodeController(editorScene.RootNode, _engineScene.RootNode, _engineScene, renderer, _meshModel);
			//2. create an EngineCameraNode in the EngineScene from ScenePreviewCamera,
			//	 set it as selected camera node in EngineScene
			//	 create a controller to transfer changes from ScenePreviewCamera
			//3. Set directed and ambient lights in the EngineScene,
			//	 create controllers to transfer changes.
			renderer.ProcessScene(_engineScene);
			renderer.Render();
		}

		public void Dispose()
		{
			_rootNodeController.Dispose();
			_engineScene.Dispose();
		}
	}
}
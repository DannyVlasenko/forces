using System;
using Forces.Models;
using Forces.Models.Render;
using ReactiveUI;
using EditorScene = Forces.Models.SceneTree.Scene;
using EngineScene = Forces.Models.Engine.Scene;
using EngineCameraNode = Forces.Models.Engine.CameraNode;
using EngineDirectedLight = Forces.Models.Engine.DirectedLight;
using Forces.Models.Engine;
using DynamicData.Binding;

namespace Forces.Controllers.Engine
{
	public class EngineSceneController : IDisposable
	{
		private readonly EngineScene _engineScene;
		private readonly EngineEmptyNodeController _rootNodeController;
		private readonly MeshModel _meshModel = new MeshModel();
		private EngineCameraNodeController _previewCameraController;
		private readonly IDisposable _previewCameraSubscription;
		private readonly IDisposable _ambientLightController;

		public EngineSceneController(EditorScene editorScene, OpenGLRenderer renderer)
		{
			_engineScene = new EngineScene();
			_rootNodeController = new EngineEmptyNodeController(editorScene.RootNode, _engineScene.RootNode, _engineScene, renderer, _meshModel);
			_previewCameraSubscription = editorScene.WhenAnyValue(x => x.PreviewCamera).Subscribe(previewCamera =>
			{
				_previewCameraController?.Dispose();
				var engineCameraNode = new EngineCameraNode(_engineScene.RootNode, previewCamera.Name);
				_previewCameraController = new EngineCameraNodeController(previewCamera, engineCameraNode, _engineScene, renderer, _meshModel);
			});
			//3. Set directed and ambient lights in the EngineScene,
			//	 create controllers to transfer changes.
			_ambientLightController = editorScene.WhenAnyValue(x => x.AmbientLight.Color).Subscribe(color =>
			{
				_engineScene.AmbientLight.Color = new Vec3(color.R, color.G, color.B);
			});
			renderer.ProcessScene(_engineScene);
			renderer.Render();
		}

		public void Dispose()
		{
			_ambientLightController.Dispose();
			_previewCameraSubscription.Dispose();
			_previewCameraController?.Dispose();
			_rootNodeController.Dispose();
			_engineScene.Dispose();
		}
	}
}
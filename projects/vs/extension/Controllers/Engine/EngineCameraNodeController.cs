using System;
using Forces.Models;
using Forces.Models.Engine;
using Forces.Models.Render;
using ReactiveUI;
using EngineScene = Forces.Models.Engine.Scene;
using EditorCameraNode = Forces.Models.SceneTree.CameraNode;
using EngineCameraNode = Forces.Models.Engine.CameraNode;

namespace Forces.Controllers.Engine
{
	public class EngineCameraNodeController : EngineNodeController
	{
		private readonly IDisposable _nearSubscription;
		private readonly IDisposable _farSubscription;
		private readonly IDisposable _fovSubscription;
		private readonly IDisposable _viewportSubscription;

		public EngineCameraNodeController(EditorCameraNode editorNode, EngineCameraNode engineNode, EngineScene scene, OpenGLRenderer renderer, MeshModel meshModel) : 
			base(editorNode, engineNode, scene, renderer, meshModel)
		{
			_nearSubscription = editorNode.WhenAnyValue(x => x.Camera.Near).Subscribe(near =>
			{
				engineNode.Camera.Near = near;
				renderer.ProcessScene(scene);
				renderer.Render();
			});

			_farSubscription = editorNode.WhenAnyValue(x => x.Camera.Far).Subscribe(far =>
			{
				engineNode.Camera.Far = far;
				renderer.ProcessScene(scene);
				renderer.Render();
			});

			_fovSubscription = editorNode.WhenAnyValue(x => x.Camera.FOV).Subscribe(fov =>
			{
				engineNode.Camera.FOV = fov;
				renderer.ProcessScene(scene);
				renderer.Render();
			});

			_viewportSubscription = editorNode.WhenAnyValue(x => x.Camera.Viewport).Subscribe(viewport =>
			{
				engineNode.Camera.Viewport = new Vec2(viewport.Width, viewport.Height);
				renderer.ProcessScene(scene);
				renderer.Render();
			});
		}

		public override void Dispose()
		{
			_nearSubscription.Dispose();
			_farSubscription.Dispose();
			_fovSubscription.Dispose();
			_viewportSubscription.Dispose();
			base.Dispose();
		}
	}
}
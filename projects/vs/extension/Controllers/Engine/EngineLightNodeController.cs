using System;
using Forces.Models;
using Forces.Models.Engine;
using Forces.Models.Render;
using ReactiveUI;
using EngineScene = Forces.Models.Engine.Scene;
using EditorLightNode = Forces.Models.SceneTree.LightNode;
using EngineLightNode = Forces.Models.Engine.LightNode;

namespace Forces.Controllers.Engine
{
	public class EngineLightNodeController : EngineNodeController
	{
		private readonly IDisposable _colorSubscription;

		public EngineLightNodeController(EditorLightNode editorNode, EngineLightNode engineNode, EngineScene scene,
			OpenGLRenderer renderer, MeshModel meshModel) :
			base(editorNode, engineNode, scene, renderer, meshModel)
		{
			_colorSubscription = editorNode.WhenAnyValue(x => x.Light.Color).Subscribe(color =>
			{
				engineNode.Light.Color = new Vec3(color.R, color.G, color.B);
				renderer.ProcessScene(scene);
				renderer.Render();
			});
		}

		public override void Dispose()
		{
			_colorSubscription.Dispose();
			base.Dispose();
		}
	}
}
using System;
using Forces.Models.Engine;
using Forces.Models.Render;
using ReactiveUI;
using EngineDirectedLight = Forces.Models.Engine.DirectedLight;
using SceneDirectedLight = Forces.Models.SceneTree.DirectedLight;
using EngineScene = Forces.Models.Engine.Scene;

namespace Forces.Controllers.Engine
{
	public class EngineDirectedLightController : IDisposable
	{
		private readonly IDisposable _nameSubscription;
		private readonly IDisposable _directionSubscription;
		private readonly IDisposable _colorSubscription;

		public EngineDirectedLightController(EngineDirectedLight engineLight, SceneDirectedLight sceneLight, EngineScene scene, OpenGLRenderer renderer)
		{
			_nameSubscription = sceneLight
				.WhenAnyValue(x => x.Name)
				.Subscribe(name =>
				{
					engineLight.Name = name;
					renderer.ProcessScene(scene);
					renderer.Render();
				});
			_directionSubscription = sceneLight
				.WhenAnyValue(x => x.Direction)
				.Subscribe(direction =>
				{
					engineLight.Direction = direction;
					renderer.ProcessScene(scene);
					renderer.Render();
				});
			_colorSubscription = sceneLight
				.WhenAnyValue(x => x.Color)
				.Subscribe(color =>
				{
					engineLight.Color = new Vec3(color.R, color.G, color.B);
					renderer.ProcessScene(scene);
					renderer.Render();
				});
		}

		public void Dispose()
		{
			_nameSubscription.Dispose();
			_directionSubscription.Dispose();
			_colorSubscription.Dispose();
		}
	}
}
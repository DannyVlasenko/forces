using System;
using System.Reactive.Linq;
using DynamicData.Binding;
using Forces.Models;
using Forces.Models.SceneTree;
using ReactiveUI;
using SceneNode = Forces.Models.SceneTree.Node;
using EngineNode = Forces.Engine.Node;
using EngineMesh = Forces.Engine.Mesh;

namespace Forces.Controllers
{
	public class EngineNodeController : IDisposable
	{
		private readonly IDisposable _nameSubscription;
		private readonly IDisposable _translationSubscription;
		private readonly IDisposable _scaleSubscription;
		private readonly IDisposable _rotationSubscription;
		private readonly IDisposable _meshSubscription;
		private readonly IDisposable _materialSubscription;

		public EngineNodeController(SceneNode sceneNode, EngineNode engineNode, RenderModel renderModel, MeshModel meshModel)
		{
			_nameSubscription = sceneNode.WhenAnyValue(x => x.Name).Subscribe(x => engineNode.Name = x);
			_translationSubscription = sceneNode.WhenAnyValue(x => x.Translation).Subscribe(x =>
			{
				engineNode.Translation = x;
				renderModel.TriggerRootNodeChanged();
			});
			//_scaleSubscription = sceneNode.WhenAnyValue(x => x.Scale).Subscribe(x => engineNode.Scale = x);
			//_rotationSubscription = sceneNode.WhenAnyValue(x => x.Rotation).Subscribe(x => engineNode.Rotation = x);

			sceneNode.Children
				.ToObservableChangeSet(x =>
				{
					if (x is MeshNode meshNode)
					{
						return new EngineNodeController(meshNode, new EngineNode(engineNode, x.Name), renderModel, meshModel);
					}
					else
					{
						return new EngineNodeController(x, new EngineNode(engineNode, x.Name), renderModel, meshModel);
					}
				})
				.Subscribe(_=>renderModel.TriggerRootNodeChanged());
		}

		public EngineNodeController(MeshNode meshNode, EngineNode engineNode, RenderModel renderModel, MeshModel meshModel) : 
			this((SceneNode)meshNode, engineNode, renderModel, meshModel)
		{
			_meshSubscription = meshNode.WhenAnyValue(x => x.Mesh)
				.Select(x => (x?.TryGetTarget(out var mesh) ?? false) ? mesh : meshModel.DefaultMesh)
				.Select(x => meshModel.EngineMeshes.TryGetValue(x, out var mesh) ? mesh : meshModel.EngineMeshes[x] = new EngineMesh(x.Path))
				.Subscribe(engineNode.SetMesh);
			//_materialSubscription = meshNode.WhenAnyValue(x => x.Material).Subscribe();
		}

		public void Dispose()
		{
			_nameSubscription?.Dispose();
			_translationSubscription?.Dispose();
			_scaleSubscription?.Dispose();
			_rotationSubscription?.Dispose();
			_meshSubscription?.Dispose();
			_materialSubscription?.Dispose();
		}
	}
}
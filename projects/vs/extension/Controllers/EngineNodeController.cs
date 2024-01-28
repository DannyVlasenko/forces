using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Forces.Models;
using Forces.Models.SceneTree;
using ReactiveUI;
using SceneNode = Forces.Models.SceneTree.Node;
using EngineNode = Forces.Engine.Node;
using SceneMesh = Forces.Models.SceneTree.Mesh;
using EngineMesh = Forces.Engine.Mesh;
using System.Reflection;

namespace Forces.Controllers
{
	public class EngineNodeController : IDisposable
	{
		private readonly SceneMesh _defaultMesh = new SceneMesh(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "sphere.obj"));
		private readonly Dictionary<SceneMesh, EngineMesh> _engineMeshes = new Dictionary<SceneMesh, EngineMesh>();
		private readonly IDisposable _nameSubscription;
		private readonly IDisposable _translationSubscription;
		private readonly IDisposable _scaleSubscription;
		private readonly IDisposable _rotationSubscription;
		private readonly IDisposable _meshSubscription;
		private readonly IDisposable _materialSubscription;

		public EngineNodeController(SceneNode sceneNode, EngineNode engineNode, RenderModel renderModel)
		{
			_nameSubscription = sceneNode.WhenAnyValue(x => x.Name).Subscribe(x => engineNode.Name = x);
			_translationSubscription = sceneNode.WhenAnyValue(x => x.Translation).Subscribe(x => engineNode.Translation = x);
			//_scaleSubscription = sceneNode.WhenAnyValue(x => x.Scale).Subscribe(x => engineNode.Scale = x);
			//_rotationSubscription = sceneNode.WhenAnyValue(x => x.Rotation).Subscribe(x => engineNode.Rotation = x);

			sceneNode.Children
				.ToObservableChangeSet(x => new EngineNodeController(x, new EngineNode(engineNode, x.Name), renderModel))
				.ToCollection()
				.Subscribe(_=>renderModel.TriggerRootNodeChanged());
		}

		public EngineNodeController(MeshNode meshNode, EngineNode engineNode, RenderModel renderModel) : 
			this((SceneNode)meshNode, engineNode, renderModel)
		{
			_meshSubscription = meshNode.WhenAnyValue(x => x.Mesh)
				.Select(x => x.TryGetTarget(out var mesh) ? mesh : _defaultMesh)
				.Select(x => _engineMeshes.TryGetValue(x, out var mesh) ? mesh : _engineMeshes[x] = new EngineMesh(x.Path))
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
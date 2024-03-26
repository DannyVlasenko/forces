using Forces.Models;
using Forces.Models.Engine;
using Forces.Models.Render;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using EngineScene = Forces.Models.Engine.Scene;
using EditorMeshNode = Forces.Models.SceneTree.MeshNode;
using EngineMeshNode = Forces.Models.Engine.MeshNode;

namespace Forces.Controllers.Engine
{
	public class EngineMeshNodeController : EngineNodeController
	{
		private readonly IDisposable _meshSubscription;
		private readonly IDisposable _materialSubscription;

		public EngineMeshNodeController(EditorMeshNode editorNode, EngineMeshNode engineNode, EngineScene scene, OpenGLRenderer renderer, MeshModel meshModel) :
			base(editorNode, engineNode, scene, renderer, meshModel)
		{
			_meshSubscription = editorNode.WhenAnyValue(x => x.Mesh)
				.Select(x => (x?.TryGetTarget(out var mesh) ?? false) ? mesh : meshModel.DefaultMesh)
				.Select(x => meshModel.EngineMeshes.TryGetValue(x, out var mesh) ? mesh : meshModel.EngineMeshes[x] = new Mesh(x.Path))
				.Subscribe(x =>
				{
					engineNode.Mesh = x;
					renderer.ProcessScene(scene);
					renderer.Render();
				});
			_materialSubscription = editorNode.WhenAnyValue(x => x.Material)
				.Select(x => (x?.TryGetTarget(out var material) ?? false) ? material : meshModel.DefaultMaterial)
				.Select(x => meshModel.EngineMaterials.TryGetValue(x, out var material) ? material : meshModel.EngineMaterials[x] = new Material() { Color = new Vec3() { X = x.Color.R / 256.0f, Y = x.Color.G / 256.0f, Z = x.Color.B / 256.0f } })
				.Subscribe(x =>
				{
					engineNode.Material = x;
					renderer.ProcessScene(scene);
					renderer.Render();
				});
		}

		public override void Dispose()
		{
			_meshSubscription.Dispose();
			_materialSubscription.Dispose();
			base.Dispose();
		}
	}
}
using Forces.Models.Render;
using Forces.Models;
using System;
using DynamicData.Binding;
using Forces.Models.Engine;
using ReactiveUI;
using EngineNode = Forces.Models.Engine.Node;
using EditorNode = Forces.Models.SceneTree.Node;
using EngineEmptyNode = Forces.Models.Engine.EmptyNode;
using EditorEmptyNode = Forces.Models.SceneTree.EmptyNode;
using EngineMeshNode = Forces.Models.Engine.MeshNode;
using EditorMeshNode = Forces.Models.SceneTree.MeshNode;
using EngineCameraNode = Forces.Models.Engine.CameraNode;
using EditorCameraNode = Forces.Models.SceneTree.CameraNode;
using EngineLightNode = Forces.Models.Engine.LightNode;
using EditorLightNode = Forces.Models.SceneTree.LightNode;

namespace Forces.Controllers.Engine
{
	public abstract class EngineNodeController : IDisposable
	{
		private readonly IDisposable _nameSubscription;
		private readonly IDisposable _translationSubscription;
		private readonly IDisposable _scaleSubscription;
		private readonly IDisposable _rotationSubscription;
		private readonly IDisposable _childrenSubscription;

		protected EngineNodeController(EditorNode editorNode, EngineNode engineNode, OpenGLRenderer renderer, MeshModel meshModel)
		{
			_nameSubscription = editorNode.WhenAnyValue(x => x.Name).Subscribe(x => engineNode.Name = x);
			_translationSubscription = editorNode.WhenAnyValue(x => x.Translation).Subscribe(translation =>
			{
				engineNode.Translation = translation;
				renderer.ProcessScene(engineNode.Scene);
				renderer.Render();
			});
			_scaleSubscription = editorNode.WhenAnyValue(x => x.Scale).Subscribe(scale =>
			{
				engineNode.Scale = scale;
				renderer.ProcessScene(engineNode.Scene);
				renderer.Render();
			});
			_rotationSubscription = editorNode.WhenAnyValue(x => x.Rotation).Subscribe(rotation =>
			{
				engineNode.Rotation = rotation;
				renderer.ProcessScene(engineNode.Scene);
				renderer.Render();
			});
			_childrenSubscription = editorNode.Children
				.ToObservableChangeSet(x =>
				{
					if (x is EditorEmptyNode editorEmptyNode)
					{
						var engineEmptyNode = new EngineEmptyNode(engineNode, x.Name);
						return new EngineEmptyNodeController(editorEmptyNode, engineEmptyNode, renderer, meshModel);
					}
					if (x is EditorMeshNode editorMeshNode)
					{
						var editorMesh = editorMeshNode.Mesh.TryGetTarget(out var edms) ? edms : meshModel.DefaultMesh;
						var engineMesh = meshModel.EngineMeshes.TryGetValue(editorMesh, out var enms)
							? enms
							: meshModel.EngineMeshes[editorMesh] = new Mesh(editorMesh.Path);
						var editorMaterial = editorMeshNode.Material.TryGetTarget(out var edmt) ? edmt : meshModel.DefaultMaterial;
						var engineMaterial = meshModel.EngineMaterials.TryGetValue(editorMaterial, out var enmt)
							? enmt
							: meshModel.EngineMaterials[editorMaterial] = new Material() { Color = new Vec3() { X = editorMaterial.Color.R, Y = editorMaterial.Color.G, Z = editorMaterial.Color.B } };
						var engineMeshNode = new EngineMeshNode(engineNode, x.Name, engineMesh, engineMaterial);
						return new EngineMeshNodeController(editorMeshNode, engineMeshNode, renderer, meshModel);
					}
					if (x is EditorCameraNode editorCameraNode)
					{
						var engineCameraNode = new EngineCameraNode(engineNode, x.Name);
						return new EngineCameraNodeController(editorCameraNode, engineCameraNode, renderer, meshModel);
					}
					if (x is EditorLightNode editorLightNode)
					{
						var engineLightNode = new EngineLightNode(engineNode, x.Name);
						return new EngineLightNodeController(editorLightNode, engineLightNode, renderer, meshModel);
					}

					throw new InvalidOperationException($"Scene node type <{x.GetType().FullName}> is not recognized.");
				})
				.Subscribe(_ =>
				{
					renderer.ProcessScene(engineNode.Scene);
					renderer.Render();
				});
		}
		public virtual void Dispose()
		{
			_nameSubscription.Dispose();
			_translationSubscription.Dispose();
			_rotationSubscription.Dispose();
			_scaleSubscription.Dispose();
			_childrenSubscription.Dispose();
		}
	}
}
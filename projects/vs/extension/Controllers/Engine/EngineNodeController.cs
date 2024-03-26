using Forces.Models.Render;
using Forces.Models;
using System;
using DynamicData.Binding;
using Forces.Models.Engine;
using ReactiveUI;
using EngineScene = Forces.Models.Engine.Scene;
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

		protected EngineNodeController(EditorNode editorNode, EngineNode engineNode, EngineScene scene, OpenGLRenderer renderer, MeshModel meshModel)
		{
			_nameSubscription = editorNode.WhenAnyValue(x => x.Name).Subscribe(x => engineNode.Name = x);
			_translationSubscription = editorNode.WhenAnyValue(x => x.Translation).Subscribe(translation =>
			{
				engineNode.Translation = translation;
				renderer.ProcessScene(scene);
				renderer.Render();
			});
			_scaleSubscription = editorNode.WhenAnyValue(x => x.Scale).Subscribe(scale =>
			{
				engineNode.Scale = scale;
				renderer.ProcessScene(scene);
				renderer.Render();
			});
			_rotationSubscription = editorNode.WhenAnyValue(x => x.Rotation).Subscribe(rotation =>
			{
				engineNode.Rotation = rotation;
				renderer.ProcessScene(scene);
				renderer.Render();
			});
			_childrenSubscription = editorNode.Children
				.ToObservableChangeSet<EditorNode, EngineNodeController>(editorChildNode =>
				{
					switch (editorChildNode)
					{
						case EditorEmptyNode editorEmptyNode:
						{
							var engineEmptyNode = new EngineEmptyNode(engineNode, editorChildNode.Name);
							return new EngineEmptyNodeController(editorEmptyNode, engineEmptyNode, scene, renderer, meshModel);
						}
						case EditorMeshNode editorMeshNode:
						{
							var editorMesh = editorMeshNode.Mesh.TryGetTarget(out var edms) ? edms : meshModel.DefaultMesh;
							var engineMesh = meshModel.EngineMeshes.TryGetValue(editorMesh, out var enms)
								? enms
								: meshModel.EngineMeshes[editorMesh] = new Mesh(editorMesh.Path);
							var editorMaterial = editorMeshNode.Material.TryGetTarget(out var edmt) ? edmt : meshModel.DefaultMaterial;
							var engineMaterial = meshModel.EngineMaterials.TryGetValue(editorMaterial, out var enmt)
								? enmt
								: meshModel.EngineMaterials[editorMaterial] = new Material() { Color = new Vec3() { X = editorMaterial.Color.R / 256.0f, Y = editorMaterial.Color.G / 256.0f, Z = editorMaterial.Color.B / 256.0f } };
							var engineMeshNode = new EngineMeshNode(engineNode, editorChildNode.Name, engineMesh, engineMaterial);
							return new EngineMeshNodeController(editorMeshNode, engineMeshNode, scene, renderer, meshModel);
						}
						case EditorCameraNode editorCameraNode:
						{
							var engineCameraNode = new EngineCameraNode(engineNode, editorChildNode.Name);
							return new EngineCameraNodeController(editorCameraNode, engineCameraNode, scene, renderer, meshModel);
						}
						case EditorLightNode editorLightNode:
						{
							var engineLightNode = new EngineLightNode(engineNode, editorChildNode.Name);
							return new EngineLightNodeController(editorLightNode, engineLightNode, scene, renderer, meshModel);
						}
						default:
							throw new InvalidOperationException($"Scene node type <{editorChildNode.GetType().FullName}> is not recognized.");
					}
				})
				.Subscribe(_ =>
				{
					renderer.ProcessScene(scene);
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
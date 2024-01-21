using System;
using Forces.Engine;
using Forces.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using SceneTreeNode = Forces.Models.SceneTree.Node;
using EngineNode = Forces.Engine.Node;

namespace Forces.Controllers
{
	public class RenderModelController
	{
		private Scene _scene = new Scene();
		private Camera _camera = new Camera();

		private List<Mesh> _meshes = new List<Mesh>
		{
			new Mesh(Path.Combine(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
				throw new FileNotFoundException("Cannot locate current assembly."), "sphere.obj"))
		};

		public RenderModelController(RenderModel renderModel, SelectionModel selectionModel)
		{
			selectionModel
				.WhenAnyValue(x => x.SelectedScene.RootNode)
				.Select(BuildRenderTree)
				.Subscribe(rn => renderModel.RootNode = rn);

			selectionModel
				.WhenAnyValue(x => x.SelectedScene.PreviewCamera.Translation)
				.Subscribe(x => renderModel.PreviewCamera.Position = new Vec3());

			renderModel.RootNode = _scene.RootNode;
			renderModel.PreviewCamera = new Camera();
		}

		private EngineNode BuildRenderTree(SceneTreeNode root)
		{
			_scene?.Dispose();
			_scene = new Scene();
			_scene.RootNode.Name = root.Name;
			BuildNodeWithChildren(root, _scene.RootNode);
			return _scene.RootNode;
		}

		private void BuildNodeWithChildren(SceneTreeNode src, EngineNode dst)
		{
			dst.Translation = new Vec3 { X = src.Translation.X, Y = src.Translation.Y, Z = src.Translation.Z };
			foreach (var srcChild in src.Children)
			{
				var engineNode = new EngineNode(dst, src.Name);
				BuildNodeWithChildren(srcChild, engineNode);
			}
		}
	}
}
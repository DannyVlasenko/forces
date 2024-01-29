using System;
using Forces.Models;
using Forces.Models.SceneTree;
using ReactiveUI;
using Scene = Forces.Engine.Scene;

namespace Forces.Controllers
{
	public class RenderModelController
	{
		private Scene _scene;
		private EngineNodeController _rootNodeController;
		private MeshModel _meshModel = new MeshModel();

		public RenderModelController(RenderModel renderModel, SelectionModel selectionModel)
		{
			selectionModel
				.WhenAnyValue(x => x.SelectedScene)
				.Subscribe(x =>
				{
					_scene = new Scene();
					_rootNodeController = new EngineNodeController(x?.RootNode ?? new EmptyNode(String.Empty), _scene.RootNode, renderModel, _meshModel);
					renderModel.RootNode = _scene.RootNode;
				});

			selectionModel
				.WhenAnyValue(x => x.SelectedScene.PreviewCamera.Translation)
				.Subscribe(x => renderModel.PreviewCamera.Position = x);
		}
	}
}
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
					if (x == null) return;
					renderModel.PreviewCamera = new Engine.Camera()
					{
						FOV = x.PreviewCamera.FOV,
						Far = x.PreviewCamera.Far,
						Near = x.PreviewCamera.Near,
						Position = x.PreviewCamera.Translation,
					};
				});

			selectionModel
				.WhenAnyValue(x => x.SelectedScene.PreviewCamera.Translation)
				.Subscribe(x =>
				{
					renderModel.PreviewCamera.Position = x;
				});

			selectionModel
				.WhenAnyValue(x => x.SelectedScene.PreviewCamera.Rotation)
				.Subscribe(x =>
				{
					renderModel.PreviewCamera.Rotation = x;
				});

			selectionModel
				.WhenAnyValue(x => x.SelectedScene.PreviewCamera.Near)
				.Subscribe(x =>
				{
					renderModel.PreviewCamera.Near = x;
				});

			selectionModel
				.WhenAnyValue(x => x.SelectedScene.PreviewCamera.Far)
				.Subscribe(x =>
				{
					renderModel.PreviewCamera.Far = x;
				});

			selectionModel
				.WhenAnyValue(x => x.SelectedScene.PreviewCamera.FOV)
				.Subscribe(x =>
				{
					renderModel.PreviewCamera.FOV = x;
				});
		}
	}
}
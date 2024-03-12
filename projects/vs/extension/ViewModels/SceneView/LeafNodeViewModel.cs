using Forces.Models.SceneTree;
using Forces.Models;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace Forces.ViewModels.SceneView
{
	public class LeafViewModel : SceneViewNodeViewModel
	{
		private readonly IDisposable _nameSubscription;
		public LeafViewModel(DirectedLight directedLightModel, SelectionModel selectionModel)
		{
			_nameSubscription = directedLightModel
				.WhenAnyValue(x => x.Name)
				.Subscribe(name => Name = name);
			this.WhenAnyValue(x => x.IsSelected)
				.Where(x => x)
				.Subscribe(_ => selectionModel.SelectedSceneViewNode = directedLightModel);
		}
		public LeafViewModel(AmbientLight ambientLightModel, SelectionModel selectionModel)
		{
			Name = "Ambient Light";
			this.WhenAnyValue(x => x.IsSelected)
				.Where(x => x)
				.Subscribe(_ => selectionModel.SelectedSceneViewNode = ambientLightModel);
		}
		public LeafViewModel(CameraNode previewCameraModel, SelectionModel selectionModel)
		{
			Name = previewCameraModel.Name;
			this.WhenAnyValue(x => x.IsSelected)
				.Where(x => x)
				.Subscribe(_ => selectionModel.SelectedSceneViewNode = previewCameraModel);
		}
	}
}
using System;
using System.Collections.ObjectModel;
using Forces.Models;
using ReactiveUI;

namespace Forces.ViewModels.SceneView
{
	public sealed class SceneViewModel : ReactiveObject
	{
		private IDisposable _sceneNameSubscription;

		private string _sceneName;

		public string SceneName
		{
			get => _sceneName;
			set => this.RaiseAndSetIfChanged(ref _sceneName, value);
		}

		public ObservableCollection<SceneViewNodeViewModel> Nodes { get; } = new ObservableCollection<SceneViewNodeViewModel>();

		public SceneViewModel(SelectionModel selectionModel)
		{
			selectionModel
				.WhenAnyValue(x => x.SelectedScene.Name)
				.Subscribe(name=>SceneName = name);
			selectionModel
				.WhenAnyValue(x => x.SelectedScene)
				.Subscribe(scene =>
				{
					_sceneNameSubscription?.Dispose();
					Nodes.Clear();
					if (scene != null)
					{
						_sceneNameSubscription = 
							this.WhenAnyValue(x => x.SceneName)
								.Subscribe(name => scene.Name = name);
						Nodes.Add(new NonLeafNodeViewModel(scene.RootNode, selectionModel));
						Nodes.Add(new NonLeafNodeViewModel(scene, selectionModel));
						Nodes.Add(new LeafViewModel(scene.AmbientLight, selectionModel));
						Nodes.Add(new LeafViewModel(scene.PreviewCamera, selectionModel));
					}
				});
			
		}
	}
}

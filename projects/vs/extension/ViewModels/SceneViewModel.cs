using System;
using System.Collections.ObjectModel;
using Forces.Models;
using ReactiveUI;

namespace Forces.ViewModels
{
	public sealed class SceneViewModel : ReactiveObject
	{
		private readonly IDisposable _sceneNameSubscription;
		private readonly IDisposable _nodesSubscription;

		private string _sceneName;

		public string SceneName
		{
			get => _sceneName;
			set => this.RaiseAndSetIfChanged(ref _sceneName, value);
		}

		public ObservableCollection<SceneViewNodeViewModel> Nodes { get; } = new ObservableCollection<SceneViewNodeViewModel>();

		public SceneViewModel(SelectionModel model)
		{
			_sceneNameSubscription = model
				.WhenAnyValue(x => x.SelectedScene.Name)
				.Subscribe(name=>SceneName = name);//ToProperty
			_nodesSubscription = model
				.WhenAnyValue(x => x.SelectedScene)
				.Subscribe(scene =>
				{
					Nodes.Clear();
					if (scene != null)
					{
						Nodes.Add(new NodeViewModel(scene.RootNode));
						Nodes.Add(new NodeViewModel(scene));
						Nodes.Add(new LeafViewModel(scene.AmbientLight));
					}
				});
		}
	}
}

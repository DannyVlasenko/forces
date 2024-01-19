using System;
using System.Collections.ObjectModel;
using DynamicData;
using Forces.Engine;
using Forces.Models;
using ReactiveUI;

namespace Forces.ViewModels
{
	public sealed class SceneViewModel : ReactiveObject
	{
		public IObservable<string> Name { get; set; }
		public ObservableCollection<SceneViewNodeViewModel> Nodes { get; }

		public SceneViewModel(SelectionModel model)
		{
			Name = model.WhenAnyValue(x => x.SelectedScene.Name);
			Nodes = model.SelectedScene.
		}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Alias;
using DynamicData.Binding;
using ReactiveUI;
using Forces.Models.SceneTree;
using Forces.Models;

namespace Forces.ViewModels
{
	public abstract class SceneViewNodeViewModel : ReactiveObject
	{
		private string _name;

		public string Name
		{
			get =>_name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		public IObservableCollection<SceneViewNodeViewModel> Children { get; } =
			new ObservableCollectionExtended<SceneViewNodeViewModel>();
		public ReactiveCommand<Unit, Unit> CreateChildCommand { get; protected set; }

		bool _isExpanded;
		public bool IsExpanded
		{
			get => _isExpanded;
			set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
		}

		bool _isSelected;
		public bool IsSelected
		{
			get => _isSelected;
			set => this.RaiseAndSetIfChanged(ref _isSelected, value);
		}
	}

	public class NodeViewModel : SceneViewNodeViewModel
	{
		private readonly IDisposable _nameSubscription;
		public NodeViewModel(Node model, SelectionModel selectionModel)
		{
			_nameSubscription = model
				.WhenAnyValue(x => x.Name)
				.Subscribe(name => Name = name);
			model
				.Children
				.ToObservableChangeSet()
				.Select(x=> new NodeViewModel(x, selectionModel) as SceneViewNodeViewModel)
				.Bind(Children)
				.Subscribe();
			CreateChildCommand = ReactiveCommand.Create(() =>
			{
				model.Children.Add(new MeshNode("Mesh_1"));
			});
			this.WhenAnyValue(x => x.IsSelected)
				.Where(x => x)
				.Subscribe(_ => selectionModel.SelectedSceneViewNode = model);
		}

		public NodeViewModel(IDirectedLightsModel directedLightModel, SelectionModel selectionModel)
		{
			Name = "Directed Lights";
			directedLightModel
				.DirectedLights
				.ToObservableChangeSet()
				.Select(x => new LeafViewModel(x, selectionModel) as SceneViewNodeViewModel)
				.Bind(Children)
				.Subscribe();
			CreateChildCommand = 
				ReactiveCommand.Create(() =>
				{
					directedLightModel
						.DirectedLights
						.Add(new DirectedLight(Color.White, Vector3.UnitZ, "DirectedLight_1"));
				});
		}
	}

	public class LeafViewModel : SceneViewNodeViewModel
	{
		private readonly IDisposable _nameSubscription;
		public LeafViewModel(DirectedLight directedLightModel, SelectionModel selectionModel)
		{
			_nameSubscription = directedLightModel
				.WhenAnyValue(x => x.Name)
				.Subscribe(name => Name = name);//ToProperty
			CreateChildCommand = ReactiveCommand.Create(() => { });
			this.WhenAnyValue(x => x.IsSelected)
				.Where(x => x)
				.Subscribe(_ => selectionModel.SelectedSceneViewNode = directedLightModel);
		}
		public LeafViewModel(AmbientLight ambientLightModel, SelectionModel selectionModel)
		{
			Name = "Ambient Light";
			CreateChildCommand = ReactiveCommand.Create(() => { });
			this.WhenAnyValue(x => x.IsSelected)
				.Where(x => x)
				.Subscribe(_ => selectionModel.SelectedSceneViewNode = ambientLightModel);
		}
	}
}

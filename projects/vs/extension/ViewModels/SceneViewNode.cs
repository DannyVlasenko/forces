using System;
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
	public class NodeAction
	{
		public NodeAction(string actionName, ReactiveCommand<Unit, Unit> actionCommand)
		{
			ActionName = actionName;
			ActionCommand = actionCommand;
		}

		public string ActionName { get; }
		public ReactiveCommand<Unit, Unit> ActionCommand { get; }
	}

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

		public IObservableCollection<NodeAction> Actions { get; } = new ObservableCollectionExtended<NodeAction>();

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
			Actions.Add(
				new NodeAction("Add Mesh Node", ReactiveCommand.Create(() =>
				{
					model.Children.Add(new MeshNode("Mesh_1"));
				})));
			Actions.Add(
				new NodeAction("Add Empty Node", ReactiveCommand.Create(() =>
				{
					model.Children.Add(new EmptyNode("Empty_1"));
				})));
			Actions.Add(
				new NodeAction("Add Camera Node", ReactiveCommand.Create(() =>
				{
					model.Children.Add(new CameraNode("Camera_1"));
				})));
			Actions.Add(
				new NodeAction("Add Light Node", ReactiveCommand.Create(() =>
				{
					model.Children.Add(new LightNode("Light_1"));
				})));
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
			Actions.Add(new NodeAction("Add Directed Light",
				ReactiveCommand.Create(() =>
				{
					directedLightModel
						.DirectedLights
						.Add(new DirectedLight(Color.White, Vector3.UnitZ, "DirectedLight_1"));
				})));
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
	}
}

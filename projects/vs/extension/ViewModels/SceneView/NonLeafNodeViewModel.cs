using Forces.Models.SceneTree;
using Forces.Models;
using System;
using System.Drawing;
using System.Numerics;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Alias;
using DynamicData.Binding;
using ReactiveUI;

namespace Forces.ViewModels.SceneView
{
	public class NonLeafNodeViewModel : SceneViewNodeViewModel
	{
		private readonly IDisposable _nameSubscription;

		public NonLeafNodeViewModel(Node model, SelectionModel selectionModel)
		{
			_nameSubscription = model
				.WhenAnyValue(x => x.Name)
				.Subscribe(name => Name = name);
			model
				.Children
				.ToObservableChangeSet()
				.Select(x => new NonLeafNodeViewModel(x, selectionModel) as SceneViewNodeViewModel)
				.Bind(Children)
				.Subscribe();
			Actions.Add(
				new NodeAction("Add Mesh Node",
					ReactiveCommand.Create(() => { model.Children.Add(new MeshNode("Mesh_1")); })));
			Actions.Add(
				new NodeAction("Add Empty Node",
					ReactiveCommand.Create(() => { model.Children.Add(new EmptyNode("Empty_1")); })));
			Actions.Add(
				new NodeAction("Add Camera Node",
					ReactiveCommand.Create(() => { model.Children.Add(new CameraNode("Camera_1")); })));
			Actions.Add(
				new NodeAction("Add Light Node",
					ReactiveCommand.Create(() => { model.Children.Add(new LightNode("Light_1")); })));
			this.WhenAnyValue(x => x.IsSelected)
				.Where(x => x)
				.Subscribe(_ => selectionModel.SelectedSceneViewNode = model);
		}

		public NonLeafNodeViewModel(IDirectedLightsModel directedLightModel, SelectionModel selectionModel)
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
}
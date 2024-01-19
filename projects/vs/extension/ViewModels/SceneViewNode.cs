using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Reactive;
using System.Windows.Input;
using DynamicData;
using DynamicData.Alias;
using DynamicData.Binding;
using ReactiveUI;
using Forces.Models;
using System.Reactive.Linq;

namespace Forces.ViewModels
{
	public abstract class SceneViewNodeViewModel : ReactiveObject
	{
		public IObservable<string> Name { get; protected set; }
		public IObservableList<SceneViewNodeViewModel> Children { get; protected set; }
		public ReactiveCommand<Unit, Unit> CreateChildCommand { get; protected set; }
	}

	public class NodeViewModel : SceneViewNodeViewModel
	{
		public NodeViewModel(Node model)
		{
			Name = model
				.WhenAnyValue(x => x.Name);
			Children = model
				.Children
				.ToObservableChangeSet()
				.Select(x=> new NodeViewModel(x) as SceneViewNodeViewModel)
				.AsObservableList();
			CreateChildCommand = ReactiveCommand.Create(() => model.Children.Add(new MeshNode("Mesh_1")));
		}

		public NodeViewModel(IDirectedLightsModel directedLightModel)
		{
			Name = Observable.Return("Directed Lights");
			Children = directedLightModel
				.DirectedLights
				.ToObservableChangeSet()
				.Select(x => new LeafViewModel(x) as SceneViewNodeViewModel)
				.AsObservableList();
			CreateChildCommand = 
				ReactiveCommand.Create(() => 
						directedLightModel
							.DirectedLights
							.Add(new DirectedLight(Color.White, Vector3.UnitZ, "DirectedLight_1"))
					);
		}
	}

	public class LeafViewModel : SceneViewNodeViewModel
	{
		public LeafViewModel(DirectedLight directedLightModel)
		{
			Name = directedLightModel
				.WhenAnyValue(x => x.Name);
			Children = new SourceList<SceneViewNodeViewModel>();
			CreateChildCommand = ReactiveCommand.Create(() => { });
		}
		public LeafViewModel(AmbientLight ambientLightModel)
		{
			Name = Observable.Return("Ambient Light");
			Children = new SourceList<SceneViewNodeViewModel>();
			CreateChildCommand = ReactiveCommand.Create(() => { });
		}
	}
}

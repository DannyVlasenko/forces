using System;
using System.Drawing;
using System.Numerics;
using System.Reactive;
using DynamicData;
using DynamicData.Alias;
using DynamicData.Binding;
using ReactiveUI;
using Forces.Models.SceneTree;

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
		public IObservableList<SceneViewNodeViewModel> Children { get; protected set; }
		public ReactiveCommand<Unit, Unit> CreateChildCommand { get; protected set; }
	}

	public class NodeViewModel : SceneViewNodeViewModel
	{
		private readonly IDisposable _nameSubscription;
		public NodeViewModel(Node model)
		{
			_nameSubscription = model
				.WhenAnyValue(x => x.Name)
				.Subscribe(name => Name = name);
			Children = model
				.Children
				.ToObservableChangeSet()
				.Select(x=> new NodeViewModel(x) as SceneViewNodeViewModel)
				.AsObservableList();
			CreateChildCommand = ReactiveCommand.Create(() => model.Children.Add(new MeshNode("Mesh_1")));
		}

		public NodeViewModel(IDirectedLightsModel directedLightModel)
		{
			Name = "Directed Lights";
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
		private readonly IDisposable _nameSubscription;
		public LeafViewModel(DirectedLight directedLightModel)
		{
			_nameSubscription = directedLightModel
				.WhenAnyValue(x => x.Name)
				.Subscribe(name => Name = name);//ToProperty
			Children = new SourceList<SceneViewNodeViewModel>();
			CreateChildCommand = ReactiveCommand.Create(() => { });
		}
		public LeafViewModel(AmbientLight ambientLightModel)
		{
			Name = "Ambient Light";
			Children = new SourceList<SceneViewNodeViewModel>();
			CreateChildCommand = ReactiveCommand.Create(() => { });
		}
	}
}

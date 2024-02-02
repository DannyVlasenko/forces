using DynamicData.Binding;
using ReactiveUI;

namespace Forces.ViewModels.SceneView
{
	public abstract class SceneViewNodeViewModel : ReactiveObject
	{
		private string _name;
		public string Name
		{
			get => _name;
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
}
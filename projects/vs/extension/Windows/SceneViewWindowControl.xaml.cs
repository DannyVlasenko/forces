using System.Reactive.Disposables;
using System.Windows;
using Forces.Models;
using Forces.ViewModels;
using ReactiveUI;

namespace Forces.Windows
{
	public sealed partial class SceneViewWindowControl : ReactiveUserControl<SceneViewModel>
	{
		private readonly SelectionModel _model;

		public SceneViewWindowControl(SelectionModel model)
		{
			_model = model;
			InitializeComponent();
			ViewModel = new SceneViewModel(model);
			this.WhenActivated(disposable =>
			{
				this.OneWayBind(ViewModel, x => x.Nodes, x => x.SceneTreeView.ItemsSource)
					.DisposeWith(disposable);
				this.Bind(ViewModel, x => x.SceneName, x => x.SceneNameTextBox.Text)
					.DisposeWith(disposable);
			});
		}
	}
}
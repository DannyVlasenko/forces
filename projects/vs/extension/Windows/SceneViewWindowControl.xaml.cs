using System.Windows;
using System.Windows.Controls;
using Forces.ViewModels;

namespace Forces.Windows
{
	public sealed partial class SceneViewWindowControl : UserControl
	{
		public SceneViewWindowControl(SceneViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}

		private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.NewValue is ISceneViewNode node)
			{
				((SceneViewModel)DataContext).SelectedNode = node;
			}
		}
	}
}
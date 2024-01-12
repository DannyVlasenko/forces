using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Forces.ViewModels;

namespace Forces.Windows
{
	public partial class SceneViewWindowControl : UserControl
	{
		public SceneViewWindowControl()
		{
			this.InitializeComponent();
		}

		public IEnumerable<ISceneViewNode> Items => SceneTreeView.Items.OfType<ISceneViewNode>();

		public ISceneViewNode SelectedItem => SceneTreeView.SelectedItem as ISceneViewNode;

		public void AddItem(ISceneViewNode item)
		{
			SceneTreeView.Items.Add(item);
		}

		public event EventHandler<ISceneViewNode> SelectedItemChanged; 
		private void SceneTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (SelectedItem != null)
			{
				SelectedItemChanged?.Invoke(this, SelectedItem);
			}
		}

		public void ClearItems()
		{
			SceneTreeView.Items.Clear();
		}
	}
}
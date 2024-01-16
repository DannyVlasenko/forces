using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forces.Engine;
using Forces.Models;

namespace Forces.ViewModels
{
	public sealed class SceneViewModel : INotifyPropertyChanged
	{
		private readonly SelectionModel _model;
		private Scene _selectedScene;
		private ISceneViewNode _selectedItem;

		public SceneViewModel(SelectionModel model)
		{
			_model = model;
			_selectedScene = _model.SelectedScene;
			if (_selectedScene != null)
			{
				_selectedScene.NodeChanged += _selectedScene_NodeChanged;
			}
			_model.SelectedSceneChanged += _model_SelectedSceneChanged;
			Nodes = new ObservableCollection<ISceneViewNode>();
			UpdateTopLevelNodes();
		}

		private void _selectedScene_NodeChanged(object sender, Node e)
		{
			UpdateTopLevelNodes();
		}

		private void _model_SelectedSceneChanged(object sender, Scene e)
		{
			if (_selectedScene != null)
			{
				_selectedScene.NodeChanged -= _selectedScene_NodeChanged;
			}
			_selectedScene = e;
			if (_selectedScene != null)
			{
				_selectedScene.NodeChanged += _selectedScene_NodeChanged;
			}
			UpdateTopLevelNodes();
			OnPropertyChanged(nameof(SceneName));
		}

		private void UpdateTopLevelNodes()
		{
			Nodes.Clear();
			if (_model.SelectedScene == null) return;
			Nodes.Add(new SceneViewNode(_model.SelectedScene.RootNode));
			Nodes.Add(new SceneViewCamera(_model.SelectedScene.PreviewCamera));
		}

		public string SceneName => _model.SceneName;

		public ISceneViewNode SelectedNode

		{
			get => _selectedItem;
			set
			{
				if (Equals(value, _selectedItem)) return;
				_selectedItem = value;
				OnPropertyChanged();
				_model.SelectSceneViewNode(_selectedItem);
			}
		}

		public ObservableCollection<ISceneViewNode> Nodes { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

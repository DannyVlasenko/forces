using System;
using Forces.Engine;
using Forces.Models;
using System.ComponentModel;

namespace Forces.ViewModels
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Serializable]
	public struct TranslationProperty
	{
		public Node Node;
		public SelectedSceneModel SceneModel;
		public float X
		{
			get => Node.Translation.X;
			set
			{
				var old = Node.Translation;
				old.X = value;
				Node.Translation = old;
				SceneModel.UpdateScene(SceneModel.SelectedScene, SceneModel.SceneName);
			}
		}
		public float Y
		{
			get => Node.Translation.Y;
			set
			{
				var old = Node.Translation;
				old.Y = value;
				Node.Translation = old;
				SceneModel.UpdateScene(SceneModel.SelectedScene, SceneModel.SceneName);
			}
		}
		public float Z
		{
			get => Node.Translation.Z;
			set
			{
				var old = Node.Translation;
				old.Z = value;
				Node.Translation = old;
				SceneModel.UpdateScene(SceneModel.SelectedScene, SceneModel.SceneName);
			}
		}

		public override string ToString()
		{
			return $"X:{X};Y:{Y};Z:{Z}";
		}
	}
	public class NodePropertiesModel
	{
		private readonly Node _node;
		private readonly SelectedSceneModel _sceneModel;

		public NodePropertiesModel(Node node, SelectedSceneModel sceneModel)
		{
			_node = node;
			_sceneModel = sceneModel;
		}

		public string Name => _node.Name;

		public bool IsVisible => _node.IsVisible;

		public TranslationProperty Translation => new TranslationProperty{Node = _node, SceneModel = _sceneModel};

		public override string ToString() => Name;
	}
}

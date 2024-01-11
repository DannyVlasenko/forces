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
		private readonly Node _sceneViewNode;
		private readonly SelectedSceneModel _sceneModel;

		public NodePropertiesModel(Node sceneViewNode, SelectedSceneModel sceneModel)
		{
			_sceneViewNode = sceneViewNode;
			_sceneModel = sceneModel;
		}

		public string Name => _sceneViewNode.Name;

		public bool IsVisible => _sceneViewNode.IsVisible;

		public TranslationProperty Translation => new TranslationProperty{Node = _sceneViewNode, SceneModel = _sceneModel};

		public override string ToString() => Name;
	}


	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Serializable]
	public struct PositionProperty
	{
		public Camera Camera;
		public SelectedSceneModel SceneModel;
		public float X
		{
			get => Camera.Position.X;
			set
			{
				var old = Camera.Position;
				old.X = value;
				Camera.Position = old;
				SceneModel.UpdatePreviewCamera(Camera);
			}
		}
		public float Y
		{
			get => Camera.Position.Y;
			set
			{
				var old = Camera.Position;
				old.Y = value;
				Camera.Position = old;
				SceneModel.UpdatePreviewCamera(Camera);
			}
		}
		public float Z
		{
			get => Camera.Position.Z;
			set
			{
				var old = Camera.Position;
				old.Z = value;
				Camera.Position = old;
				SceneModel.UpdatePreviewCamera(Camera);
			}
		}

		public override string ToString()
		{
			return $"X:{X};Y:{Y};Z:{Z}";
		}
	}

	public class CameraPropertiesModel
	{
		private readonly Camera _camera;
		private readonly SelectedSceneModel _sceneModel;

		public CameraPropertiesModel(Camera camera, SelectedSceneModel sceneModel)
		{
			_camera = camera;
			_sceneModel = sceneModel;
		}

		public string Name => "Camera";

		public bool IsVisible => true;

		public float Near
		{
			get => _camera.Near;
			set
			{
				_camera.Near = value; 
				_sceneModel.UpdatePreviewCamera(_camera);
			}
		}

		public float Far
		{
			get => _camera.Far;
			set
			{
				_camera.Far = value; 
				_sceneModel.UpdatePreviewCamera(_camera);
			}
		}

		public PositionProperty Position => new PositionProperty(){Camera = _camera, SceneModel = _sceneModel};

		public override string ToString() => Name;
	}
}

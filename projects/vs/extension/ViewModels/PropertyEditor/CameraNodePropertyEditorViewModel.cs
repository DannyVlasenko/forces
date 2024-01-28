using System.Drawing;
using Forces.Models.SceneTree;

namespace Forces.ViewModels.PropertyEditor
{
	public class CameraNodePropertyEditorViewModel : NodePropertyEditorViewModel
	{
		private readonly Camera _camera;
		public CameraNodePropertyEditorViewModel(CameraNode node) : base(node)
		{
			_camera = node.Camera;
		}

		public float Near
		{
			get => _camera.Near;
			set => _camera.Near = value;
		}

		public float Far
		{
			get => _camera.Far;
			set => _camera.Far = value;
		}

		public float FOV
		{
			get => _camera.FOV;
			set => _camera.FOV = value;
		}

		public Size Viewport
		{
			get => _camera.Viewport;
			set => _camera.Viewport = value;
		}
	}
}
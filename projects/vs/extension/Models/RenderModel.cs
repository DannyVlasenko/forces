using Forces.Engine;

namespace Forces.Models
{
	public class RenderModel : ModelObjectWithNotifications
	{
		private Camera _previewCamera = new Camera();
		private Node _rootNode;

		public Camera PreviewCamera
		{
			get => _previewCamera;
			set => SetField(ref _previewCamera, value);
		}

		public Node RootNode
		{
			get => _rootNode;
			set => SetField(ref _rootNode, value);
		}

		public void TriggerRootNodeChanged()
		{
			OnPropertyChanged(nameof(RootNode));
		}
	}
}
using System.Drawing;

namespace Forces.Models
{
	public class Camera : ModelObjectWithNotifications
	{
		private float _fov;
		private float _near;
		private float _far;
		private Size _viewport;

		public float FOV
		{
			get => _fov;
			set => SetField(ref _fov, value);
		}

		public float Near
		{
			get => _near;
			set => SetField(ref _near, value);
		}

		public float Far
		{
			get => _far;
			set => SetField(ref _far, value);
		}

		public Size Viewport
		{
			get => _viewport;
			set => SetField(ref _viewport, value);
		}
	}
}

using System.Numerics;

namespace Forces.Models
{
	public class PreviewCamera : Camera
	{
		private Vector3 _translation = Vector3.Zero;
		private Quaternion _rotation = Quaternion.Identity;

		public Vector3 Translation
		{
			get => _translation;
			set => SetField(ref _translation, value);
		}

		public Quaternion Rotation
		{
			get => _rotation;
			set => SetField(ref _rotation, value);
		}
	}
}

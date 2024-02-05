﻿using System;
using Forces.Models;
using Forces.Models.SceneTree;
using Forces.Utilities;
using ReactiveUI;

namespace Forces.ViewModels.PropertyEditor
{
	public class PreviewCameraPropertyEditorViewModel : ModelObjectWithNotifications
	{
		private readonly PreviewCamera _camera;

		public PreviewCameraPropertyEditorViewModel(PreviewCamera camera)
		{
			_camera = camera;
			_camera.WhenAnyValue(x => x.Rotation)
				.Subscribe(_ => OnPropertyChanged(nameof(Rotation)));
		}

		public Vector3Property Translation => new Vector3Property(() => _camera.Translation, tr => _camera.Translation = tr);

		public Vector3Property Rotation => new Vector3Property(() => _camera.Rotation.ToEulerXYZ(), rt => _camera.Rotation = rt.EulerXYZToQuaternion());
		
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
	}
}
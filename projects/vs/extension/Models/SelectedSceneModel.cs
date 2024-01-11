using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EnvDTE;
using EnvDTE80;
using Forces.Engine;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json.Linq;

namespace Forces.Models
{
	public class SelectedSceneModel
	{
		public Scene SelectedScene { get; private set; }
		public string SceneName { get; private set; }

		public event EventHandler<Scene> SelectedSceneChanged;
		public event EventHandler<Camera> CameraChanged;

		public void UpdateScene(Scene scene, string name)
		{
			SelectedScene = scene;
			SceneName = name;
			SelectedSceneChanged?.Invoke(this, SelectedScene);
			
		}

		public void UpdatePreviewCamera(Camera camera)
		{
			SelectedScene.PreviewCamera = camera;
			CameraChanged?.Invoke(this, camera);
		}
	}
}

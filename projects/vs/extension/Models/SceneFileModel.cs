using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Forces.Models.SceneTree;

namespace Forces.Models
{
	public class SceneFileModel
	{
		private readonly Dictionary<string, Scene> _loadedScenes = new Dictionary<string, Scene>();
		public Scene SceneForFile(string path)
		{
			if (!_loadedScenes.ContainsKey(path))
			{
				//Load from file
				//Subscribe scene file changed
				var scene = new Scene(Path.GetFileNameWithoutExtension(path));
				scene.PreviewCamera.Camera.FOV = 60;
				scene.PreviewCamera.Camera.Near = 1;
				scene.PreviewCamera.Camera.Far = 100;
				scene.PreviewCamera.Camera.Viewport = new Size(100, 100);
				_loadedScenes.Add(path, scene);
			}

			return _loadedScenes[path];
		}
	}
}

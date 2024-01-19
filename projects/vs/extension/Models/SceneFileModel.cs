using System.Collections.Generic;
using System.IO;

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
				_loadedScenes.Add(path, new Scene(Path.GetFileNameWithoutExtension(path)));
			}

			return _loadedScenes[path];
		}
	}
}

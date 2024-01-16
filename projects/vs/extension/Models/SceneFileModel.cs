using System.Collections.Generic;
using Forces.Engine;

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
				//Subscribe scene changed
				_loadedScenes.Add(path, new Scene());
			}

			return _loadedScenes[path];
		}
	}
}

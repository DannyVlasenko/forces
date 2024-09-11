using System.Collections.Generic;

namespace Forces.Models
{
	public class AssetFileModel
	{
		private readonly Dictionary<string, AssetStorage> _loadedAssetStorages = new Dictionary<string, AssetStorage>();

		public AssetStorage AssetStorageForFile(string file)
		{
			if (!_loadedAssetStorages.ContainsKey(file))
			{

			}
			return _loadedAssetStorages[file];
		}
	}
}
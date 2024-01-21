namespace Forces.Models.SceneTree
{
	public class Mesh : ModelObjectWithNotifications
	{
		private string _path;
		private string _name;

		public Mesh(string name)
		{
			_name = name;
		}

		public string Name
		{
			get => _name;
			set => SetField(ref _name, value);
		}

		public string Path
		{
			get => _path;
			set => SetField(ref _path, value);
		}
	}
}

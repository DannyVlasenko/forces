using System.Collections.ObjectModel;
using System.Drawing;

namespace Forces.Models.SceneTree
{
	public class Scene : ModelObjectWithNotifications, IDirectedLightsModel
	{
		private string _name;

		public Scene(string name)
		{
			_name = name;
		}

		public string Name
		{
			get => _name;
			set => SetField(ref _name, value);
		}

		public EmptyNode RootNode { get; } = new EmptyNode("Root");

		public CameraNode PreviewCamera { get; } = new CameraNode("Preview Camera");

		public AmbientLight AmbientLight { get; } = new AmbientLight(Color.White);

		public ObservableCollection<DirectedLight> DirectedLights { get; } = new ObservableCollection<DirectedLight>();
	}
}

using System.Collections.ObjectModel;

namespace Forces.Models.SceneTree
{
	public interface IDirectedLightsModel
	{
		ObservableCollection<DirectedLight> DirectedLights { get; }
	}
}

using System.Collections.ObjectModel;

namespace Forces.Models
{
	public interface IDirectedLightsModel
	{
		ObservableCollection<DirectedLight> DirectedLights { get; }
	}
}

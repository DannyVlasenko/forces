using System.Collections.ObjectModel;
using System;

namespace Forces.Models
{
	public class MeshNode : Node
	{
		public MeshNode(string name) : base(name) {}
		public WeakReference<Material> Material { get; set; }
		public ObservableCollection<WeakReference<Mesh>> Meshes { get; } = new ObservableCollection<WeakReference<Mesh>>();
	}
}

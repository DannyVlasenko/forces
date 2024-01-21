using System;
using System.Collections.ObjectModel;

namespace Forces.Models.SceneTree
{
	public class MeshNode : Node
	{
		public MeshNode(string name) : base(name) {}
		public WeakReference<Material> Material { get; set; }
		public ObservableCollection<WeakReference<Mesh>> Meshes { get; } = new ObservableCollection<WeakReference<Mesh>>();
	}
}

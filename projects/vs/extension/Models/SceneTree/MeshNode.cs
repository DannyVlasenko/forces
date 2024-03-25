using System;

namespace Forces.Models.SceneTree
{
	public class MeshNode : Node
	{
		private WeakReference<Material> _material = new WeakReference<Material>(null);
		private WeakReference<Mesh> _mesh = new WeakReference<Mesh>(null);

		public MeshNode(string name) : base(name) {}

		public WeakReference<Material> Material
		{
			get => _material;
			set => SetField(ref _material, value);
		}

		public WeakReference<Mesh> Mesh
		{
			get => _mesh;
			set => SetField(ref _mesh, value);
		}
	}
}

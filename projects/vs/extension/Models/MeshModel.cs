using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using SceneMesh = Forces.Models.SceneTree.Mesh;
using EngineMesh = Forces.Models.Engine.Mesh;
using SceneMaterial = Forces.Models.SceneTree.Material;
using EngineMaterial = Forces.Models.Engine.Material;

namespace Forces.Models
{
	public class MeshModel
	{
		public SceneMaterial DefaultMaterial { get; } = new SceneMaterial("Default Material", Color.BurlyWood);

		public Dictionary<SceneMaterial, EngineMaterial> EngineMaterials { get; } = new Dictionary<SceneMaterial, EngineMaterial>();

		public SceneMesh DefaultMesh { get; } = new SceneMesh("Default Mesh"){Path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "sphere.obj")};

		public Dictionary<SceneMesh, EngineMesh> EngineMeshes { get; } = new Dictionary<SceneMesh, EngineMesh>();
	}
}
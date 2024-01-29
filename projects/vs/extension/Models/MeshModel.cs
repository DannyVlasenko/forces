using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SceneMesh = Forces.Models.SceneTree.Mesh;
using EngineMesh = Forces.Engine.Mesh;

namespace Forces.Models
{
	public class MeshModel
	{
		public SceneMesh DefaultMesh { get; } = new SceneMesh("Default Mesh"){Path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "sphere.obj")};

		public Dictionary<SceneMesh, EngineMesh> EngineMeshes { get; } = new Dictionary<SceneMesh, EngineMesh>();
	}
}
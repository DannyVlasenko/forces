using Forces.Models;
using Forces.Models.Render;
using EngineScene = Forces.Models.Engine.Scene;
using EditorLightNode = Forces.Models.SceneTree.LightNode;
using EngineLightNode = Forces.Models.Engine.LightNode;

namespace Forces.Controllers.Engine
{
	public class EngineLightNodeController : EngineNodeController
	{
		public EngineLightNodeController(EditorLightNode editorNode, EngineLightNode engineNode, EngineScene scene, OpenGLRenderer renderer, MeshModel meshModel) : 
			base(editorNode, engineNode, scene, renderer, meshModel)
		{}
	}
}
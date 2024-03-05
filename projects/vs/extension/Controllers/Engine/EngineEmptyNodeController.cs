using Forces.Models;
using Forces.Models.Render;
using EngineScene = Forces.Models.Engine.Scene;
using EngineEmptyNode = Forces.Models.Engine.EmptyNode;
using EditorEmptyNode = Forces.Models.SceneTree.EmptyNode;

namespace Forces.Controllers.Engine
{
	public class EngineEmptyNodeController : EngineNodeController
	{
		public EngineEmptyNodeController(EditorEmptyNode editorEmptyNode, EngineEmptyNode engineEmptyNode, EngineScene scene, OpenGLRenderer renderer, MeshModel meshModel) : 
			base(editorEmptyNode, engineEmptyNode, scene, renderer, meshModel)
		{}
	}
}
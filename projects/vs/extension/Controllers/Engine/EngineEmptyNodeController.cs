using Forces.Models;
using Forces.Models.Render;
using EngineEmptyNode = Forces.Models.Engine.EmptyNode;
using EditorEmptyNode = Forces.Models.SceneTree.EmptyNode;

namespace Forces.Controllers.Engine
{
	public class EngineEmptyNodeController : EngineNodeController
	{
		public EngineEmptyNodeController(EditorEmptyNode editorEmptyNode, EngineEmptyNode engineEmptyNode, OpenGLRenderer renderer, MeshModel meshModel) : 
			base(editorEmptyNode, engineEmptyNode, renderer, meshModel)
		{}
	}
}
using Forces.Models;
using Forces.Models.Render;
using EngineScene = Forces.Models.Engine.Scene;
using EditorCameraNode = Forces.Models.SceneTree.CameraNode;
using EngineCameraNode = Forces.Models.Engine.CameraNode;

namespace Forces.Controllers.Engine
{
	public class EngineCameraNodeController : EngineNodeController
	{
		public EngineCameraNodeController(EditorCameraNode editorNode, EngineCameraNode engineNode, EngineScene scene, OpenGLRenderer renderer, MeshModel meshModel) : 
			base(editorNode, engineNode, scene, renderer, meshModel)
		{
			
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
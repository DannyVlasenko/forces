using Forces.Models;
using Forces.Models.Render;
using ReactiveUI;
using EditorCameraNode = Forces.Models.SceneTree.CameraNode;
using EngineCameraNode = Forces.Models.Engine.CameraNode;

namespace Forces.Controllers.Engine
{
	public class EngineCameraNodeController : EngineNodeController
	{
		public EngineCameraNodeController(EditorCameraNode editorNode, EngineCameraNode engineNode, OpenGLRenderer renderer, MeshModel meshModel) : 
			base(editorNode, engineNode, renderer, meshModel)
		{
			
		}

		public override void Dispose()
		{
			
			base.Dispose();
		}
	}
}
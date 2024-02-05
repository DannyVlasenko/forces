using System;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	public class OpenGLRenderer : IDisposable
	{
		private IntPtr _renderer = create_opengl_renderer();

		public void SetCurrentRootNode(Node root)
		{
			opengl_renderer_set_root_node(_renderer, root.Handle);
		}
		public void SetCamera(Camera camera)
		{
			opengl_renderer_set_camera(_renderer, camera.Handle);
		}

		public void Render()
		{
			opengl_renderer_render(_renderer);
		}

		private void ReleaseUnmanagedResources()
		{
			delete_opengl_renderer(_renderer);
			_renderer = IntPtr.Zero;
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~OpenGLRenderer()
		{
			ReleaseUnmanagedResources();
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_opengl_renderer();

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void delete_opengl_renderer(IntPtr renderer);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void opengl_renderer_render(IntPtr renderer);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void opengl_renderer_set_root_node(IntPtr renderer, IntPtr node);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void opengl_renderer_set_camera(IntPtr renderer, IntPtr camera);
	}
}

using System;
using System.Runtime.InteropServices;
using Forces.Models.Engine;

namespace Forces.Models.Render
{
	public class OpenGLRenderer : IDisposable
	{
		private readonly Window _window;
		private IntPtr _renderer;

		public OpenGLRenderer(RenderWindow window)
		{
			_window = new Window(window);
			_renderer = create_opengl_renderer(_window.Handle);
			window.Paint += Window_Paint;
		}

		private void Window_Paint(object sender, EventArgs e)
		{
			Render();
		}

		public void ProcessScene(Scene scene)
		{
			opengl_renderer_process_scene(_renderer, scene.Handle);
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
			_window.Dispose();
			GC.SuppressFinalize(this);
		}

		~OpenGLRenderer()
		{
			ReleaseUnmanagedResources();
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_opengl_renderer(IntPtr window);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void delete_opengl_renderer(IntPtr renderer);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void opengl_renderer_render(IntPtr renderer);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void opengl_renderer_process_scene(IntPtr renderer, IntPtr scene);

	}
}

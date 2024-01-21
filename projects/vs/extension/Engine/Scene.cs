using System;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	public class Scene : IDisposable
	{
		private IntPtr _handle = create_scene();

		public Node RootNode => new Node(scene_root_node(_handle));

		private void ReleaseUnmanagedResources()
		{
			delete_scene(_handle);
			_handle = IntPtr.Zero;
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~Scene()
		{
			ReleaseUnmanagedResources();
		}


		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_scene();

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void delete_scene(IntPtr scene);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr scene_root_node(IntPtr scene);
	}
}

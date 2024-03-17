using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class Scene : IDisposable
	{
		public IntPtr Handle { get; private set; } = create_scene();

		public EmptyNode RootNode => new EmptyNode(scene_root_node(Handle));

		public AmbientLight AmbientLight => new AmbientLight(scene_ambient_light(Handle));

		public CameraNode ActiveCameraNode
		{
			set => scene_set_active_camera_node(Handle, value.Handle);
		}

		public DirectedLight AddDirectedLight(string name)
		{
			return new DirectedLight(scene_create_directed_light(Handle, name));
		}

		public void RemoveDirectedLight(DirectedLight light)
		{
			scene_remove_directed_light(Handle, light.Handle);
		}

		private void ReleaseUnmanagedResources()
		{
			delete_scene(Handle);
			Handle = IntPtr.Zero;
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

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr scene_ambient_light(IntPtr scene);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr scene_create_directed_light(IntPtr scene, string name);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void scene_remove_directed_light(IntPtr scene, IntPtr light);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void scene_set_active_camera_node(IntPtr scene, IntPtr cameraNode);
	}
}

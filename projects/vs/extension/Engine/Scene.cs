﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	public class Scene : IDisposable
	{
		private IntPtr _handle;

		public Scene()
		{
			_handle = create_scene();
			Meshes = new List<Mesh>();
			var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			Meshes.Add(new Mesh(Path.Combine(dir, "sphere.obj")));
			var sphereNode = new Node(RootNode, "Sphere1")
			{
				Translation = new Vec3() { X = 0, Y = 0, Z = 3 }
			};
			sphereNode.AddMesh(Meshes[0]);
			PreviewCamera = new Camera();
		}

		public event EventHandler<Node> NodeChanged;

		public Node RootNode => new Node(scene_root_node(_handle), "RootNode", this);

		public IList<Mesh> Meshes { get; private set; }

		public Camera PreviewCamera { get; set; }

		public void NotifyNodeChanged(Node node)
		{
			NodeChanged?.Invoke(this, node);
		}

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

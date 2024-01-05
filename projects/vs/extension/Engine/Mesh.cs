using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Engine
{
	internal class Mesh : IDisposable
	{
		private IntPtr _mesh;

		public string Path => Marshal.PtrToStringUni(mesh_get_path(_mesh));

		public Mesh(string path)
		{
			_mesh = create_mesh(path);
		}

		private void ReleaseUnmanagedResources()
		{
			delete_mesh(_mesh);
			_mesh = IntPtr.Zero;
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~Mesh()
		{
			ReleaseUnmanagedResources();
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_mesh(string path);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void delete_mesh(IntPtr mesh);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr mesh_get_path(IntPtr mesh);
	}
}

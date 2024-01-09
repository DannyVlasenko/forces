using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Engine
{
	public class Mesh : IDisposable
	{
		public IntPtr Handle { get; private set; }

		public string Path => Marshal.PtrToStringUni(mesh_get_path(Handle));

		public Mesh(string path)
		{
			Handle = create_mesh(path);
		}

		private void ReleaseUnmanagedResources()
		{
			delete_mesh(Handle);
			Handle = IntPtr.Zero;
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

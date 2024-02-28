using System;
using System.Runtime.InteropServices;

namespace Forces.Models.Engine
{
	public class Material : IDisposable
	{
		private void ReleaseUnmanagedResources()
		{
			delete_material(Handle);
			Handle = IntPtr.Zero;
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~Material()
		{
			ReleaseUnmanagedResources();
		}
		public IntPtr Handle { get; private set; } = create_material();

		public Vec3 Color
		{
			set => material_set_color(Handle, value);
		}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_material();

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void delete_material(IntPtr material);

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern void material_set_color(IntPtr material, Vec3 color);
	}
}

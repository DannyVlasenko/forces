using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace Forces.Windows
{
	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	/// </summary>
	/// <remarks>
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
	/// usually implemented by the package implementer.
	/// <para>
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its
	/// implementation of the IVsUIElementPane interface.
	/// </para>
	/// </remarks>
	[Guid("606da3ec-a27c-4e36-b3fd-4d13f23204df")]
	public class SceneViewWindow : ToolWindowPane
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SceneViewWindow"/> class.
		/// </summary>
		public SceneViewWindow() : base(null)
		{
			this.Caption = "SceneViewWindow";

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
			// the object returned by the Content property.
			this.Content = new SceneViewWindowControl(this);
		}
		internal object GetVsService(Type service)
		{
			return GetService(service);
		}
	}
}

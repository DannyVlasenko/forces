using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using Forces.Engine;

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
	[Guid("5e8a8814-5f59-48fd-ade2-911513fcc6e2")]
	public class PreviewWindow : ToolWindowPane
	{
		private readonly Window _window;
		/// <summary>
		/// Initializes a new instance of the <see cref="PreviewWindow"/> class.
		/// </summary>
		public PreviewWindow() : base(null)
		{
			this.Caption = "Forces Preview";
			_window = new Window();
			this.Content = _window;
		}


	}
}

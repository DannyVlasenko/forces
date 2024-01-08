using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Forces.Engine;
using Forces.ViewModels;
using Forces.Windows;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace Forces
{
	/// <summary>
	/// Interaction logic for SceneViewWindowControl.
	/// </summary>
	public partial class SceneViewWindowControl : UserControl
	{

		private readonly SceneViewWindow _parent;
		private readonly Scene _scene;

		/// <summary>
		/// Initializes a new instance of the <see cref="SceneViewWindowControl"/> class.
		/// </summary>
		public SceneViewWindowControl(SceneViewWindow parent)
		{
			_parent = parent;
			this.InitializeComponent();
			//SceneTreeView.Items.Add(_scene.RootNode);
		}

		/// <summary>
		/// Handles click on the button by displaying a message box.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		[SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
		private void button1_Click(object sender, RoutedEventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			if (textBox1.Text.Length > 0)
			{
				//var item = new Node(textBox1.Text);
				//item.Children.Add(new Node(textBox1.Text + "_child"));
				//item.Children[0].Children.Add(new Node(textBox1.Text + "_subchild"));
				//SceneTreeView.Items.Add(item);
				//var outputWindow = (IVsOutputWindow)_parent.GetVsService(typeof(SVsOutputWindow));
				//var guidGeneralPane = VSConstants.GUID_OutWindowGeneralPane;
				//outputWindow.GetPane(ref guidGeneralPane, out var pane);
				//pane?.OutputStringThreadSafe($"Node created: {item}\r\n");
				//TrackSelection();
				////CheckForErrors();
			}
		}

		private SelectionContainer _mySelContainer;
		private System.Collections.ArrayList _mySelItems;
		private IVsWindowFrame _frame = null;

		private void TrackSelection()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			if (_frame == null)
			{
				if (_parent.GetVsService(typeof(SVsUIShell)) is IVsUIShell shell)
				{
					var guidPropertyBrowser = new Guid(ToolWindowGuids.PropertyBrowser);
					shell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guidPropertyBrowser, out _frame);
				}
			}

			_frame?.Show();
			if (_mySelContainer == null)
			{
				_mySelContainer = new SelectionContainer();
			}

			_mySelItems = new System.Collections.ArrayList();

			if (SceneTreeView.SelectedItem is Node selected)
			{
				_mySelItems.Add(new NodePropertiesModel(selected));
			}

			_mySelContainer.SelectedObjects = _mySelItems;

			if (_parent.GetVsService(typeof(STrackSelection)) is ITrackSelection track)
			{
				track.OnSelectChange(_mySelContainer);
			}
		}

		private void SceneTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			TrackSelection();
		}
	}
}
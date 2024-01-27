using System;
using System.Reactive.Linq;
using Forces.Engine;
using Forces.Models;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;
using System.Runtime.InteropServices;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace Forces.Windows
{
	[Guid("5e8a8814-5f59-48fd-ade2-911513fcc6e2")]
	public sealed class PreviewWindow : ToolWindowPane
	{
		private OpenGLRenderer _renderer;

		public PreviewWindow(SelectionModel selectionModel, RenderModel renderModel) : base(null)
		{
			selectionModel
				.WhenAnyValue(x => x.SelectedScene.Name)
				.Subscribe(name =>
				{
					Caption = "Forces Preview - " + (string.IsNullOrEmpty(name) ? "No Scene Selected" : name);
				});

			var preferences = (Preferences)(Package as ForcesPackage)?.GetDialogPage(typeof(Preferences));
			var multisampling = preferences?.PreviewMultiSampling ?? 1;
			var window = new RenderWindow(multisampling);
			window
				.Events().ContextInitialized
				.Take(1)
				.Subscribe(_ =>
				{
					window.MakeContextCurrent();
					_renderer = new OpenGLRenderer();
				});
			window
				.Events().Paint
				.Subscribe(_ =>
				{
					window.MakeContextCurrent();
					_renderer?.Render();
					window.SwapBuffers();
				});
			window.Events().SizeChanged
				.Merge(window.Events().Loaded.Take(1))
				.Subscribe(_ =>
				{
					if (renderModel.PreviewCamera == null)
					{
						return;
					}

					renderModel.PreviewCamera.Viewport = new Vec2()
					{
						X = (float)(window.RenderSize.Width * window.GetDpiXScale()),
						Y = (float)(window.RenderSize.Height * window.GetDpiYScale())
					};
				});
			renderModel
				.WhenAnyValue(x => x.RootNode)
				.WhereNotNull()
				.Subscribe(node =>
				{
					window.MakeContextCurrent();
					_renderer?.SetCurrentRootNode(node);
					window.SwapBuffers();
				});
			renderModel
				.WhenAnyValue(x => x.PreviewCamera)
				.WhereNotNull()
				.Subscribe(camera =>
				{
					window.MakeContextCurrent();
					camera.Viewport = new Vec2()
					{
						X = (float)(window.RenderSize.Width * window.GetDpiXScale()),
						Y = (float)(window.RenderSize.Height * window.GetDpiYScale())
					};
					_renderer?.SetCamera(camera);
					window.SwapBuffers();
				});
			Content = window;
		}
	}
}

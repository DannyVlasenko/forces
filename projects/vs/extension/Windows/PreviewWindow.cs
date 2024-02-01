using System;
using System.Reactive.Linq;
using Forces.Engine;
using Forces.Models;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Forces.Controllers;
using ReactiveUI;

namespace Forces.Windows
{
	public class WindowContext
	{
		public WindowContext(SelectionModel selectionModel, RenderModel renderModel)
		{
			SelectionModel = selectionModel;
			RenderModel = renderModel;
		}

		public SelectionModel SelectionModel { get; }
		public RenderModel RenderModel { get; }
	}

	[Guid("5e8a8814-5f59-48fd-ade2-911513fcc6e2")]
	public sealed class PreviewWindow : ToolWindowPane
	{
		private OpenGLRenderer _renderer;
		private PreviewCameraMovementController _cameraMovementController;

		public PreviewWindow(WindowContext context):
			this(context.SelectionModel, context.RenderModel)
		{}
		public PreviewWindow(SelectionModel selectionModel, RenderModel renderModel) : 
			base(null)
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
			selectionModel.WhenAnyValue(x => x.SelectedScene.PreviewCamera)
				.Subscribe(camera =>
				{
					_cameraMovementController?.Dispose();
					_cameraMovementController = new PreviewCameraMovementController(window, camera);
				});

			Observable.FromEventPattern<EventHandler, EventArgs>(
					h => window.ContextInitialized += h, 
					h => window.ContextInitialized -= h)
				.Take(1)
				.Subscribe(_ =>
				{
					window.MakeContextCurrent();
					_renderer = new OpenGLRenderer();
				});
			Observable.FromEventPattern<EventHandler, EventArgs>(
					h => window.Paint += h,
					h => window.Paint -= h)
				.Subscribe(_ =>
				{
					window.MakeContextCurrent();
					_renderer?.Render();
					window.SwapBuffers();
				});
			Observable.FromEventPattern<SizeChangedEventHandler, SizeChangedEventArgs>(
					h => window.SizeChanged += h,
					h => window.SizeChanged -= h)
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
			Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
					h => window.Loaded += h,
					h => window.Loaded -= h)
				.Take(1)
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
			renderModel.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "RootNode")
				{
					if (renderModel.RootNode != null)
					{
						window.MakeContextCurrent();
						_renderer?.SetCurrentRootNode(renderModel.RootNode);
						_renderer?.Render();
						window.SwapBuffers();
					}
				}
			};
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
					_renderer?.Render();
					window.SwapBuffers();
				});
			Content = window;
		}
	}
}

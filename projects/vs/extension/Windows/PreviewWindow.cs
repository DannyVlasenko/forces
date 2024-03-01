using System;
using System.Reactive.Linq;
using Forces.Models;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using Forces.Controllers;
using Forces.Controllers.Engine;
using Forces.Models.Render;
using ReactiveUI;

namespace Forces.Windows
{

	[Guid("5e8a8814-5f59-48fd-ade2-911513fcc6e2")]
	public sealed class PreviewWindow : ToolWindowPane
	{
		private PreviewCameraMovementController _cameraMovementController;
		private PreviewCameraViewportController _cameraViewportController;
		private EngineSceneController _engineSceneController;
		private OpenGLRenderer _renderer;

		public PreviewWindow(SelectionModel selectionModel) : 
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
			window.ContextInitialized += (sender, args) =>
			{
				_renderer = new OpenGLRenderer(window);
				if (selectionModel.SelectedScene != null)
				{
					_engineSceneController?.Dispose();
					_engineSceneController = new EngineSceneController(selectionModel.SelectedScene, _renderer);
				}
			};
			selectionModel.WhenAnyValue(x => x.SelectedScene.PreviewCamera)
				.WhereNotNull()
				.Subscribe(camera =>
				{
					_cameraMovementController?.Dispose();
					_cameraViewportController?.Dispose();
					_cameraMovementController = new PreviewCameraMovementController(window, camera);
					_cameraViewportController = new PreviewCameraViewportController(window, camera);
				});
			selectionModel
				.WhenAnyValue(x => x.SelectedScene)
				.WhereNotNull()
				.Where(_=>_renderer != null)
				.Subscribe(editorScene =>
				{
					_engineSceneController?.Dispose();
					_engineSceneController = new EngineSceneController(editorScene, _renderer);
				});
			Content = window;
		}
	}
}

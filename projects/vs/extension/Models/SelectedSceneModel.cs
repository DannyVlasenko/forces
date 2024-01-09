using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EnvDTE;
using EnvDTE80;
using Forces.Engine;
using Microsoft.VisualStudio.Shell;

namespace Forces.Models
{
	public class SelectedSceneModel
	{
		private Scene _selectedScene;

		public Scene SelectedScene
		{
			get => _selectedScene;
			set
			{
				_selectedScene = value; 
				SelectedSceneChanged?.Invoke(this, _selectedScene);
			}
		}

		public event EventHandler<Scene> SelectedSceneChanged;
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Forces.Engine;

namespace Forces.Scene
{
	internal class Node : INode
	{
		private readonly SceneViewWindowControl _control;

		public Node(SceneViewWindowControl control, string itemName)
		{
			_control = control;
			Name = itemName;
			Children = new List<INode>();
		}

		[Description("Name")]
		public string Name { get; set; }

		[Description("Visible")]
		public bool IsVisible { get; set; }

		public override string ToString()
		{
			return Name;
		}

		public IList<INode> Children { get; private set; }
	}
}

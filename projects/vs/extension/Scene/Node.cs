using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Scene
{
	internal class Node
	{
		private readonly SceneViewWindowControl _control;

		public Node(SceneViewWindowControl control, string itemName)
		{
			_control = control;
			Name = itemName;
		}
		[Description("Name")]
		public string Name { get; set; }

		[Description("Visible")]
		public bool IsVisible { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}

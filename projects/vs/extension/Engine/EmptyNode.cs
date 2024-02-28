﻿using System;
using System.Runtime.InteropServices;

namespace Forces.Engine
{
	public class EmptyNode : Node
	{
		public EmptyNode(Node parent, string name) : 
			base(create_empty_node(parent.Handle, name))
		{}

		[DllImport("editor.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr create_empty_node(IntPtr parent, string name);
	}
}

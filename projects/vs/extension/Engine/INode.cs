using System.Collections.Generic;

namespace Forces.Engine
{
	internal interface INode
	{
		string Name { get; }

		bool IsVisible { get; }

		IList<INode> Children { get; }
	}
}

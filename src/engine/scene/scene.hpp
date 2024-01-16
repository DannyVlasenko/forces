#pragma once
#include "node.hpp"

namespace forces
{
	class Scene
	{
	public:
		Node& rootNode() noexcept
		{
			return rootNode_;
		}

		const Node& rootNode() const noexcept
		{
			return rootNode_;
		}
		
	private:
		Node rootNode_{L"Root"};
	};
}

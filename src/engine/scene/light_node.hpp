#pragma once

#include "light.hpp"
#include "node.hpp"

namespace forces
{
	class LightNode : public Node
	{
	public:
		using Node::Node;

		[[nodiscard]]
		PointLight & light() noexcept
		{
			return light_;
		}

		[[nodiscard]]
		const PointLight & light() const noexcept
		{
			return light_;
		}

	private:
		PointLight light_;
	};
}

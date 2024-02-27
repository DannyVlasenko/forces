#pragma once

#include "node.hpp"
#include "camera.hpp"

namespace forces
{
	class CameraNode : public Node
	{
	public:
		using Node::Node;

		[[nodiscard]]
		Camera& camera() noexcept
		{
			return camera_;
		}

		[[nodiscard]]
		const Camera& camera() const noexcept
		{
			return camera_;
		}

	private:
		Camera camera_;
	};
}

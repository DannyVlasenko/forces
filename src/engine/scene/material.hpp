#pragma once
#include <vec3.hpp>

namespace forces
{
	class Material
	{
	public:
		[[nodiscard]]
		glm::vec3& color() noexcept { return color_; }

		[[nodiscard]]
		const glm::vec3& color() const noexcept { return color_; }

	private:
		glm::vec3 color_{ 0.0f, 0.0f, 0.0f };
	};
}

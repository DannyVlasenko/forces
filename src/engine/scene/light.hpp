#pragma once
#include <vec3.hpp>

namespace forces
{
	class Light
	{
	public:
		[[nodiscard]]
		glm::vec3& color() noexcept { return color_; }

		[[nodiscard]]
		const glm::vec3& color() const noexcept { return color_; }

	protected:
		Light() = default;

	private:
		glm::vec3 color_{ 0.0f, 0.0f, 0.0f };
	};

	class AmbientLight final : public Light {};

	class DirectedLight final : public Light
	{
	public:
		[[nodiscard]]
		glm::vec3& direction() noexcept { return direction_; }

		[[nodiscard]]
		const glm::vec3& direction() const noexcept { return direction_; }

	private:
		glm::vec3 direction_{ 1.0f, 0.0f, 0.0f };
	};

	class PointLight final : public Light
	{
	public:
		[[nodiscard]]
		float& strength() noexcept { return strength_; }

		[[nodiscard]]
		const float& strength() const noexcept { return strength_; }
	private:
		float strength_{ 1.0f };
	};
}

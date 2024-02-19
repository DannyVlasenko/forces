#pragma once
#include <vec3.hpp>

namespace forces
{
	class Light
	{
	public:
		glm::vec3& color() noexcept { return color_; }
		const glm::vec3& color() const noexcept { return color_; }
	protected:
		Light() = default;
	private:
		glm::vec3 color_{ 0.0f, 0.0f, 0.0f };
	};

	class AmbientLight : Light {};

	class DirectedLight : Light
	{
	public:
		glm::vec3& direction() noexcept { return direction_; }
		const glm::vec3& direction() const noexcept { return direction_; }
	private:
		glm::vec3 direction_{ 1.0f, 0.0f, 0.0f };
	};

	class PointLight : Light
	{
	public:
		glm::vec3& position() noexcept { return position_; }
		const glm::vec3& position() const noexcept { return position_; }
	private:
		glm::vec3 position_{ 0.0f, 0.0f, 0.0f };
	};
}

#ifndef MODELS_CAMERA_HPP
#define MODELS_CAMERA_HPP

#include "glm.hpp"

namespace models
{
	class Camera
	{
	public:
		glm::vec3& position() noexcept
		{
			return mPosition;
		}

		glm::vec3& rotation() noexcept
		{
			return mRotation;
		}

		float& fov() noexcept
		{
			return mFOV;
		}

		glm::vec2& viewport() noexcept
		{
			return mViewport;
		}

		float& near() noexcept
		{
			return mNear;
		}

		float& far() noexcept
		{
			return mFar;
		}

		glm::vec3 front() const noexcept;

		glm::vec3 up() const noexcept;

		glm::vec3 right() const noexcept;

		void look_at(const glm::vec3& at) noexcept;

		glm::mat4 view_projection() const;

	private:
		glm::vec3 mPosition{ 0, 0, 0 };
		glm::vec3 mRotation{ 0, 0, 0 };
		float mFOV{ 60.0f };
		glm::vec2 mViewport{ 800.0f , 600.0f };
		float mNear{ 0.1f };
		float mFar{ 10.0f };
	};
}

#endif // MODELS_CAMERA_HPP

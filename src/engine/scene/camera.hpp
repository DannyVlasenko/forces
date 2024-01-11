#ifndef MODELS_CAMERA_HPP
#define MODELS_CAMERA_HPP

#include "glm.hpp"
#include "gtc/quaternion.hpp"

namespace forces
{
	class Camera
	{
	public:
		glm::vec3& position() noexcept
		{
			return mPosition;
		}

		glm::quat& rotation() noexcept
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
		glm::quat mRotation{ 1.0f, 0.0f, 0.0f, 0.0f };
		float mFOV{ 60.0f };
		glm::vec2 mViewport{ 1920.0f , 1080.0f };
		float mNear{ 0.1f };
		float mFar{ 100.0f };
	};

	void yaw(Camera& camera, float grad) noexcept;

	void pitch(Camera& camera, float grad) noexcept;

	void roll(Camera& camera, float grad) noexcept;
}

#endif // MODELS_CAMERA_HPP

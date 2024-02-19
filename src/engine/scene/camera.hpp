#ifndef MODELS_CAMERA_HPP
#define MODELS_CAMERA_HPP

#include "glm.hpp"
#include "gtc/quaternion.hpp"

namespace forces
{
	class Camera
	{
	public:
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

		glm::mat4 view_projection(const glm::vec3& pos, const glm::vec3& front, const glm::vec3& up) const;

	private:
		float mFOV{ 60.0f };
		glm::vec2 mViewport{ 1920.0f , 1080.0f };
		float mNear{ 0.1f };
		float mFar{ 100.0f };
	};
}

#endif // MODELS_CAMERA_HPP

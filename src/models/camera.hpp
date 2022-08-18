#ifndef MODELS_CAMERA_HPP
#define MODELS_CAMERA_HPP

#include "glm.hpp"

namespace models
{
	class Camera
	{
	public:
		glm::vec3 & look_at() noexcept
		{
			return mLookAt;
		}

		glm::vec3& translation() noexcept
		{
			return mTranslation;
		}

		glm::vec3& up() noexcept
		{
			return mUp;
		}

		float & fov() noexcept
		{
			return mFOV;
		}

		glm::vec2 & viewport() noexcept
		{
			return mViewport;
		}

		float & near() noexcept
		{
			return mNear;
		}

		float & far() noexcept
		{
			return mFar;
		}

		glm::mat4 view_projection() const;

	private:
		glm::vec3 mLookAt{ 0, 0, 0 };
		glm::vec3 mTranslation{ 0, 0, 0 };
		glm::vec3 mUp{ 0, 1, 0 };
		float mFOV{ 60.0f };
		glm::vec2 mViewport{ 800.0f , 600.0f };
		float mNear{ 0.1f };
		float mFar{ 10.0f };
	};
}

#endif // MODELS_CAMERA_HPP

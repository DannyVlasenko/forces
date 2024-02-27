#ifndef MODELS_CAMERA_HPP
#define MODELS_CAMERA_HPP

#include "glm.hpp"
#include "gtc/quaternion.hpp"

namespace forces
{
	class Camera
	{
	public:
		[[nodiscard]]
		float fov() const noexcept
		{
			return mFOV;
		}

		[[nodiscard]]
		float& fov() noexcept
		{
			return mFOV;
		}

		[[nodiscard]]
		const glm::vec2& viewport() const noexcept
		{
			return mViewport;
		}

		[[nodiscard]]
		glm::vec2& viewport() noexcept
		{
			return mViewport;
		}

		[[nodiscard]]
		float near() const noexcept
		{
			return mNear;
		}

		[[nodiscard]]
		float& near() noexcept
		{
			return mNear;
		}

		[[nodiscard]]
		float far() const noexcept
		{
			return mFar;
		}

		[[nodiscard]]
		float& far() noexcept
		{
			return mFar;
		}

	private:
		float mFOV{ 60.0f };
		glm::vec2 mViewport{ 1920.0f , 1080.0f };
		float mNear{ 0.1f };
		float mFar{ 100.0f };
	};
}

#endif // MODELS_CAMERA_HPP

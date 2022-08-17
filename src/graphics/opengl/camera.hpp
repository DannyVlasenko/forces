#ifndef OPENGL_CAMERA_HPP
#define OPENGL_CAMERA_HPP

#include "glm.hpp"

namespace opengl
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

		glm::mat4 view_matrix() const;

	private:
		glm::vec3 mLookAt{ 0, 0, 0 };
		glm::vec3 mTranslation{ 0, 0, 0 };
		glm::vec3 mUp{ 0, 1, 0 };
	};
}

#endif // OPENGL_CAMERA_HPP

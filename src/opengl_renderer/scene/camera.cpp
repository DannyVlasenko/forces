#include "camera.hpp"

#include "gtc/matrix_transform.hpp"
#include "gtx/euler_angles.hpp"

namespace opengl
{
    glm::vec3 Camera::front() const noexcept
    {        
        return mRotation * glm::vec3{ 0.0f, 0.0f, 1.0f };
    }

    glm::vec3 Camera::up() const noexcept
    {
        return mRotation * glm::vec3{ 0.0f, 1.0f, 0.0f };
    }

    glm::vec3 Camera::right() const noexcept
    {
        return mRotation * glm::vec3{ 1.0f, 0.0f, 0.0f };
    }

    void Camera::look_at(const glm::vec3& at) noexcept
    {
        const auto direction = mPosition - at;
    }

    glm::mat4 Camera::view_projection() const
	{
		return glm::perspective(glm::radians(mFOV), mViewport.x / mViewport.y, mNear, mFar)
			 * glm::lookAt(mPosition, mPosition + front(), up());
	}
}

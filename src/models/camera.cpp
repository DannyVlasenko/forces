#include "camera.hpp"

#include "gtc/matrix_transform.hpp"

namespace models
{
    glm::vec3 Camera::front() const noexcept
    {
        return normalize(mLookAt);
    }

    glm::vec3 Camera::up() const noexcept
    {
        return mUp;
    }

    glm::vec3 Camera::right() const noexcept
    {
        constexpr glm::vec3 worldUp{ 0.0f, 0.1f, 0.0f };
        return normalize(cross(front(), mUp));
    }

    void Camera::yaw(float grad) noexcept
    {
        mLookAt = rotate(glm::mat4{ 1.0f }, glm::radians(grad), mUp) * glm::vec4{ mLookAt, 1.0f };
    }

    float Camera::yaw() const noexcept
    {
        return TODO_IMPLEMENT_ME;
    }

    void Camera::pitch(float grad) noexcept
    {
        mLookAt = rotate(glm::mat4{ 1.0f }, glm::radians(grad), right()) * glm::vec4{ mLookAt, 1.0f };
        mUp = normalize(cross(right(), front()));
    }

    float Camera::pitch() const noexcept
    {
        return TODO_IMPLEMENT_ME;
    }

    void Camera::roll(float grad) noexcept
    {
        mLookAt = rotate(glm::mat4{ 1.0f }, glm::radians(grad), front()) * glm::vec4{ mLookAt, 1.0f };
        mUp = normalize(cross(right(), front()));
    }

    float Camera::roll() const noexcept
    {
        return TODO_IMPLEMENT_ME;
    }

    glm::mat4 Camera::view_projection() const
	{
		return glm::perspective(glm::radians(mFOV), mViewport.x / mViewport.y, mNear, mFar)
			 * glm::lookAt(mPosition, mLookAt, mUp);
	}
}

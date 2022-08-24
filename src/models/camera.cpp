#include "camera.hpp"

#include "gtc/matrix_transform.hpp"

namespace models
{
    template <size_t Index>
    static glm::vec3 camera_axis(glm::vec3 grads) 
    {
        auto rotationX = rotate(glm::mat4{ 1.0f }, glm::radians(grads.y), glm::vec3{ 0.0f, 1.0f, 0.0f });
        auto rotationXY = rotate(rotationX, glm::radians(grads.x), glm::vec3{ 1.0f, 0.0f, 0.0f });
        auto rotationXYZ = rotate(rotationXY, glm::radians(grads.z), glm::vec3{ 0.0f, 0.0f, 1.0f });
        glm::vec4 axis{ 0.0 };
        axis[Index] = 1.0f;
        return (rotationXYZ * axis);
    }

    glm::vec3 Camera::front() const noexcept
    {        
        return camera_axis<2>(mRotation);
    }

    glm::vec3 Camera::up() const noexcept
    {
        return camera_axis<1>(mRotation);
    }

    glm::vec3 Camera::right() const noexcept
    {
        return camera_axis<0>(mRotation);
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

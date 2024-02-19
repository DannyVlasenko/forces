#include "camera.hpp"

#include "gtc/matrix_transform.hpp"
#include "gtx/euler_angles.hpp"

namespace forces
{

    glm::mat4 Camera::view_projection(const glm::vec3 &pos, const glm::vec3& front, const glm::vec3& up) const
	{
		return glm::perspective(glm::radians(mFOV), mViewport.x / mViewport.y, mNear, mFar)
			 * glm::lookAt(pos, pos + front, up);
	}
}

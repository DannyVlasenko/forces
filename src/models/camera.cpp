#include "camera.hpp"

#include "gtc/matrix_transform.hpp"

namespace models
{
	glm::mat4 Camera::view_projection() const
	{
		return glm::perspective(glm::radians(mFOV), mViewport.x / mViewport.y, mNear, mFar)
			 * glm::lookAt(mTranslation, mLookAt, mUp);
	}
}

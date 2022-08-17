#include "camera.hpp"

#include <ext/matrix_transform.hpp>

namespace opengl
{
	glm::mat4 Camera::view_matrix() const
	{
		return lookAt(mTranslation, mLookAt, mUp);
	}
}

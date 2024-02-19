#pragma once
#include <vec3.hpp>

namespace opengl
{
	struct PointLight
	{
		glm::vec3 Position;
		glm::vec3 Color;
	};

	struct DirectedLight
	{
		glm::vec3 Direction;
		glm::vec3 Color;
	};

	struct AmbientLight
	{
		glm::vec3 Color;
	};
}

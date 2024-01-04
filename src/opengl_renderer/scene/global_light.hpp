#pragma once

#include "glm.hpp"

namespace opengl 
{
	class GlobalLight
	{
	public:

		glm::vec3& ambient_color() noexcept
		{
			return mAmbientLightColor;
		}

		float& ambient_strength() noexcept
		{
			return mAmbientLightStrength;
		}

		glm::vec3& point_color() noexcept
		{
			return mPointLightColor;
		}

		glm::vec3& point_position() noexcept
		{
			return mPointLightPosition;
		}

		glm::vec3& directed_color() noexcept
		{
			return mDirectedLightColor;
		}

		glm::vec3& directed_orientation() noexcept
		{
			return mDirectedLightOrientation;
		}

		float& directed_strength() noexcept
		{
			return mDirectedLightStrength;
		}

	private:
		glm::vec3 mAmbientLightColor;
		float mAmbientLightStrength;
		glm::vec3 mPointLightPosition;
		glm::vec3 mPointLightColor;
		glm::vec3 mDirectedLightOrientation;
		glm::vec3 mDirectedLightColor;
		float mDirectedLightStrength;
	};
}

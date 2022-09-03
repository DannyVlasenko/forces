#ifndef LOGIC_GLOBAL_LIGHT_VIEW_MODEL_HPP
#define LOGIC_GLOBAL_LIGHT_VIEW_MODEL_HPP

#include "global_light_view.hpp"
#include "opengl/shader.hpp"
#include "camera.hpp"

namespace view_models 
{
	class GlobalLightViewModel final : public views::IGlobalLightViewModel
	{
	public:
		GlobalLightViewModel(opengl::Program& shader, models::Camera &camera) :
			mShader(shader),
			mCamera(camera)
		{}

		bool& ambient_light_enabled() noexcept override
		{
			return mAmbientEnabled;
		}

		glm::vec3& ambient_color() noexcept override
		{
			return mAmbientColor;
		}

		float& ambient_strength() noexcept override
		{
			return mAmbientStrength;
		}

		bool& point_light_enabled() noexcept override
		{
			return mPointEnabled;
		}

		bool& point_follow_camera() noexcept override
		{
			return mPointFollowCamera;
		}

		glm::vec3& point_color() noexcept override
		{
			return mPointColor;
		}

		glm::vec3& point_position() noexcept override
		{
			return mPointPosition;
		}

		bool& directed_light_enabled() noexcept override
		{
			return mDirectedEnabled;
		}

		glm::vec3& directed_color() noexcept override
		{
			return mDirectedColor;
		}

		glm::vec3& directed_orientation() noexcept override
		{
			return mDirectedOrientation;
		}

		void update();

	private:
		opengl::Program& mShader;
		models::Camera& mCamera;
		bool mAmbientEnabled{ true };
		glm::vec3 mAmbientColor{ 1.0f };
		float mAmbientStrength{ 0.2f };
		bool mPointEnabled{ false };
		bool mPointFollowCamera{ true };
		glm::vec3 mPointColor{ 1.0f };
		glm::vec3 mPointPosition{ 0.0f };
		bool mDirectedEnabled{ true };
		glm::vec3 mDirectedColor{ 1.0f };
		glm::vec3 mDirectedOrientation{ 1.0f };
	};
}
#endif // LOGIC_GLOBAL_LIGHT_VIEW_MODEL_HPP

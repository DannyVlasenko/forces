#include "global_light_view.hpp"
#include "opengl/shader.hpp"

namespace view_models 
{
	class GlobalLightViewModel final : public views::IGlobalLightViewModel
	{
	public:
		GlobalLightViewModel(opengl::Program& shader) :
			mShader(shader)
		{
		}

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

		bool& diffuse_light_enabled() noexcept override
		{
			return mDiffuseEnabled;
		}

		glm::vec3& diffuse_color() noexcept override
		{
			return mDiffuseColor;
		}

		glm::vec3& diffuse_position() noexcept override
		{
			return mDiffusePosition;
		}

		void update();

	private:
		opengl::Program& mShader;
		bool mAmbientEnabled{ true };
		glm::vec3 mAmbientColor{ 1.0f };
		float mAmbientStrength{ 1.0f };
		bool mDiffuseEnabled{ true };
		glm::vec3 mDiffuseColor{ 1.0f };
		glm::vec3 mDiffusePosition{ 0.0f };
	};
}

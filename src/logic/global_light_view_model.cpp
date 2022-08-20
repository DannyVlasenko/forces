#include "global_light_view_model.hpp"

namespace view_models
{
	void GlobalLightViewModel::update()
	{
		const auto ambientColor = mAmbientEnabled ? mAmbientColor * mAmbientStrength : glm::vec3(0.0f);
		mShader.set_uniform_3f("ambientLightColor", ambientColor.x, ambientColor.y, ambientColor.z);
		const auto diffuseColor = mDiffuseEnabled ? mDiffuseColor : glm::vec3(0.0f);
		mShader.set_uniform_3f("diffuseLightColor", diffuseColor.x, diffuseColor.y, diffuseColor.z);
		mShader.set_uniform_3f("diffuseLightPosition", mDiffusePosition.x, mDiffusePosition.y, mDiffusePosition.z);
	}
}
#include "global_light_view_model.hpp"

namespace view_models
{
	void GlobalLightViewModel::update()
	{
		const auto ambientColor = mAmbientEnabled ? mAmbientColor * mAmbientStrength : glm::vec3(0.0f);
		mShader.set_uniform("ambientLightColor", ambientColor);
		const auto pointColor = mPointEnabled ? mPointColor : glm::vec3(0.0f);
		mShader.set_uniform("pointLightColor", pointColor);
		const auto pointPosition = mPointFollowCamera ? mCamera.position() : mPointPosition;
		mShader.set_uniform("pointLightPosition", pointPosition);
		const auto directedColor = mDirectedEnabled ? mDirectedColor : glm::vec3(0.0f);
		mShader.set_uniform("directedLightColor", directedColor);
		mShader.set_uniform("directedLightOrient", mDirectedOrientation);
	}
}
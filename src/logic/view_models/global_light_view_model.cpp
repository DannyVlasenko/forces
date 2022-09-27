#include "global_light_view_model.hpp"

namespace view_models
{
	void GlobalLightViewModel::update() const
    {
		const auto ambientColor = mAmbientEnabled ? mAmbientColor * mAmbientStrength : glm::vec3(0.0f);
		mShader.setAmbientLightColor(ambientColor);
		const auto pointColor = mPointEnabled ? mPointColor : glm::vec3(0.0f);
		mShader.setPointLightColor(pointColor);
		const auto pointPosition = mPointFollowCamera ? mCamera.position() : mPointPosition;
		mShader.setPointLightPosition(pointPosition);
		const auto directedColor = mDirectedEnabled ? mDirectedColor : glm::vec3(0.0f);
		mShader.setDirectedLightColor(directedColor);
		mShader.setDirectedLightOrientation(mDirectedOrientation);
	}
}
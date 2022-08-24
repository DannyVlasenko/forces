#include "opengl/glcall.hpp"
#include "camera_view_model.hpp"

namespace view_models 
{
	void CameraViewModel::update()
	{
		mWindow.set_swap_interval(mVSync);
		if (mViewportMatchWindow)
		{
			mCamera.viewport() = mWindow.framebuffer_size();
		}
		GLCall(glViewport(0, 0, mCamera.viewport().x, mCamera.viewport().y));
		GLCall(glClearColor(mClearColor.x, mClearColor.y, mClearColor.z, 1.0f));
		GLCall(glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT));
	}
}
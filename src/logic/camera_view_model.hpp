#pragma once

#include "camera_view.hpp"
#include "camera.hpp"
#include "glfw/window.hpp"

namespace view_models
{
	class CameraViewModel final : public views::ICameraViewModel
	{
	public:
		CameraViewModel(models::Camera& camera, const glfw::Window& window) :
			mCamera(camera),
			mWindow(window)
		{}

		glm::vec3& look_at() noexcept override { return mCamera.look_at(); }
		glm::vec3& translation() noexcept override { return mCamera.translation(); }
		glm::vec3& up() noexcept override { return mCamera.up(); }
		float& fov() noexcept override { return mCamera.fov(); }
		glm::vec2& viewport() noexcept override { return mCamera.viewport(); }
		float& near() noexcept override { return mCamera.near(); }
		float& far() noexcept override { return mCamera.far(); }
		bool& v_sync() noexcept override { return mVSync; }
		bool& viewport_match_window() noexcept override { return mViewportMatchWindow; }
		glm::vec3& clear_color() noexcept override { return mClearColor; }

		void update();

	private:
		bool mVSync{ true };
		bool mViewportMatchWindow{ true };
		glm::vec3 mClearColor{ 0.2f, 0.2f, 0.2f };
		models::Camera& mCamera;
		const glfw::Window& mWindow;
	};
}
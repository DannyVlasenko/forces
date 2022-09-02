#ifndef LOGIC_CAMERA_VIEW_MODEL_HPP
#define LOGIC_CAMERA_VIEW_MODEL_HPP

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

		glm::vec3& position() noexcept override { return mCamera.position(); }
		glm::quat& rotation() noexcept override { return mCamera.rotation(); }
		glm::vec3 up() const noexcept override { return mCamera.up(); }
		glm::vec3 front() const noexcept override { return mCamera.front(); }
		glm::vec3 right() const noexcept override { return mCamera.right(); }
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
#endif // LOGIC_CAMERA_VIEW_MODEL_HPP

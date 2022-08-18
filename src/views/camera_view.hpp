#ifndef VIEWS_CAMERA_VIEW_HPP
#define VIEWS_CAMERA_VIEW_HPP
#include <glm.hpp>

#include "view.hpp"

namespace views
{
	class ICameraViewModel
	{
	public:
		virtual ~ICameraViewModel() = default;

		virtual glm::vec3& look_at() noexcept = 0;
		virtual glm::vec3& translation() noexcept = 0;
		virtual glm::vec3& up() noexcept = 0;
		virtual float& fov() noexcept = 0;
		virtual glm::vec2& viewport() noexcept = 0;
		virtual float& near() noexcept = 0;
		virtual float& far() noexcept = 0;
		virtual bool& v_sync() noexcept = 0;
		virtual bool& viewport_match_window() noexcept = 0;
		virtual glm::vec4& clear_color() noexcept = 0;
	};

	class CameraView final : public IView
	{
	public:
		CameraView(ICameraViewModel &view_model):
			mViewModel(view_model)
		{}

		void render() override;

	private:
		ICameraViewModel &mViewModel;
	};
}
#endif // VIEWS_CAMERA_VIEW_HPP

#ifndef VIEWS_GLOBAL_LIGHT_VIEW_HPP
#define VIEWS_GLOBAL_LIGHT_VIEW_HPP

#include "view.hpp"
#include "glm.hpp"

namespace views
{
	class IGlobalLightViewModel
	{
	public:
		virtual ~IGlobalLightViewModel() = default;

		virtual bool& ambient_light_enabled() noexcept = 0;
		virtual glm::vec3& ambient_color() noexcept = 0;
		virtual float& ambient_strength() noexcept = 0;
		virtual bool& point_light_enabled() noexcept = 0;
		virtual bool& point_follow_camera() noexcept = 0;
		virtual glm::vec3& point_color() noexcept = 0;
		virtual glm::vec3& point_position() noexcept = 0;
		virtual bool& directed_light_enabled() noexcept = 0;
		virtual glm::vec3& directed_color() noexcept = 0;
		virtual glm::vec3& directed_orientation() noexcept = 0;
	};

	class GlobalLightView final : public IView
	{
	public:
		GlobalLightView(IGlobalLightViewModel& view_model) :
			mViewModel(view_model)
		{}

		void render() override;

	private:
		IGlobalLightViewModel& mViewModel;
	};
}
#endif // VIEWS_GLOBAL_LIGHT_VIEW_HPP

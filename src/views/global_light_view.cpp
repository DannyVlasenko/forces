#include "global_light_view.hpp"

#include "imgui.h"

namespace views
{
	void GlobalLightView::render()
	{
		ImGui::Begin("Global light");

		//Ambient
		ImGui::Checkbox("Ambient light", &mViewModel.ambient_light_enabled());
		if (!mViewModel.ambient_light_enabled())
		{
			ImGui::BeginDisabled();
		}
		ImGui::ColorEdit3("Ambient color", &mViewModel.ambient_color()[0]);
		ImGui::DragFloat("Ambient strength", &mViewModel.ambient_strength(), 0.005f, 0.0f, 1.0f);
		if (!mViewModel.ambient_light_enabled())
		{
			ImGui::EndDisabled();
		}

		//Point
		ImGui::Checkbox("Point light", &mViewModel.point_light_enabled());
		if (!mViewModel.point_light_enabled())
		{
			ImGui::BeginDisabled();
		}
		ImGui::SameLine();
		ImGui::Checkbox("Follow camera", &mViewModel.point_follow_camera());
		ImGui::ColorEdit3("Point light color", &mViewModel.point_color()[0]);
		ImGui::DragFloat3("Point light position", &mViewModel.point_position()[0]);
		if (!mViewModel.point_light_enabled())
		{
			ImGui::EndDisabled();
		}

		//Directed
		ImGui::Checkbox("Directed light", &mViewModel.directed_light_enabled());
		if (!mViewModel.directed_light_enabled())
		{
			ImGui::BeginDisabled();
		}
		ImGui::ColorEdit3("Directed light color", &mViewModel.directed_color()[0]);
		ImGui::DragFloat3("Direction", &mViewModel.directed_orientation()[0]);
		if (!mViewModel.directed_light_enabled())
		{
			ImGui::EndDisabled();
		}

		ImGui::End();
	}
}

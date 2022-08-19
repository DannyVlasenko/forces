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

		//Diffuse
		ImGui::Checkbox("Diffuse light", &mViewModel.diffuse_light_enabled());
		if (!mViewModel.diffuse_light_enabled())
		{
			ImGui::BeginDisabled();
		}
		ImGui::ColorEdit3("Diffuse color", &mViewModel.diffuse_color()[0]);
		ImGui::DragFloat3("Diffuse position", &mViewModel.diffuse_position()[0]);
		if (!mViewModel.diffuse_light_enabled())
		{
			ImGui::EndDisabled();
		}

		ImGui::End();
	}
}

#include "camera_view.hpp"

#include "imgui.h"

namespace views
{
	void CameraView::render()
	{
		ImGui::Begin("Camera");
		ImGui::DragFloat3("Translation", &mViewModel.translation()[0]);
		ImGui::DragFloat3("Look At", &mViewModel.look_at()[0]);
		ImGui::DragFloat3("Up", &mViewModel.up()[0]);
		ImGui::DragFloat("FOV", &mViewModel.fov());
		ImGui::DragFloat("Near", &mViewModel.near());
		ImGui::DragFloat("Far", &mViewModel.far());
		if (mViewModel.viewport_match_window()) {
			ImGui::BeginDisabled();
		}
		ImGui::DragFloat2("Viewport", &mViewModel.viewport()[0]);
		if (mViewModel.viewport_match_window()) {
			ImGui::EndDisabled();
		}
		ImGui::Checkbox("Match window size", &mViewModel.viewport_match_window());
		ImGui::ColorEdit3("Clear color", &mViewModel.clear_color()[0]);
		ImGui::Checkbox("Vertical sync", &mViewModel.v_sync());
		ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);
		ImGui::End();
	}
}

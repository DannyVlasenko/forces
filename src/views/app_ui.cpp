#include "app_ui.hpp"

#include "imgui.h"
#include "imgui_impl_glfw.h"
#include "imgui_impl_opengl3.h"

views::AppUI::AppUI(const glfw::Window& window)
{
	ImGui::CreateContext();
	ImGui::StyleColorsDark();
	ImGui_ImplGlfw_InitForOpenGL(window, true);
	ImGui_ImplOpenGL3_Init("#version 330");
}

views::AppUI::~AppUI()
{
	ImGui_ImplGlfw_Shutdown();
	ImGui::DestroyContext();
}

void views::AppUI::add_view(std::unique_ptr<IView> view)
{
	mViews.push_back(std::move(view));
}

void views::AppUI::render()
{
	ImGui_ImplOpenGL3_NewFrame();
	ImGui_ImplGlfw_NewFrame();
	ImGui::NewFrame();

	for (const auto& view : mViews)
	{
		view->render();
	}

	ImGui::Render();
	ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());
}

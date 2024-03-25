#include "opengl_renderer.h"

#include "opengl_renderer/renderer.hpp"

struct ForcesWindow : forces::Window
{
	explicit ForcesWindow(GLFWwindow* window)
		: window_(window)
	{}

	void makeContextCurrent() const override
	{
		glfwMakeContextCurrent(window_);
	}

	void swapBuffers() const override
	{
		glfwSwapBuffers(window_);
	}
private:
	GLFWwindow* window_;
};

ForcesWindow* adapt_glfw_window(GLFWwindow* glfwWindow)
{
	return new ForcesWindow(glfwWindow);
}

void delete_window_adapter(ForcesWindow* window)
{
	delete window;
}

ForcesOpenGLRenderer* create_opengl_renderer(ForcesWindow* window)
{
	return reinterpret_cast<ForcesOpenGLRenderer*>(new opengl::Renderer(*window));
}

void delete_opengl_renderer(ForcesOpenGLRenderer* renderer)
{
	delete reinterpret_cast<opengl::Renderer*>(renderer);
}

void opengl_renderer_process_scene(ForcesOpenGLRenderer* renderer, ForcesScene* scene)
{
	reinterpret_cast<opengl::Renderer*>(renderer)->processScene(*reinterpret_cast<forces::Scene*>(scene));
}

void opengl_renderer_render(ForcesOpenGLRenderer* renderer)
{
	reinterpret_cast<opengl::Renderer*>(renderer)->render();
}

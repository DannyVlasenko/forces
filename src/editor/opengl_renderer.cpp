#include "opengl_renderer.h"

#include "engine/scene/camera.hpp"
#include "engine/scene/node.hpp"
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

ForcesOpenGLRenderer* create_opengl_renderer()
{
	return reinterpret_cast<ForcesOpenGLRenderer*>(new opengl::Renderer);
}

void delete_opengl_renderer(ForcesOpenGLRenderer* renderer)
{
	delete reinterpret_cast<opengl::Renderer*>(renderer);
}

void opengl_renderer_set_root_node(ForcesOpenGLRenderer* renderer, ForcesNode* root)
{
	reinterpret_cast<opengl::Renderer*>(renderer)->setCurrentRootNode(*reinterpret_cast<forces::Node*>(root));
}

void opengl_renderer_render(ForcesOpenGLRenderer* renderer)
{
	reinterpret_cast<opengl::Renderer*>(renderer)->render();
}

void opengl_renderer_set_camera(ForcesOpenGLRenderer* renderer, ForcesCamera* camera)
{
	reinterpret_cast<opengl::Renderer*>(renderer)->setCamera(reinterpret_cast<forces::Camera *>(camera));
}

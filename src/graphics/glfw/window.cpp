#include "window.hpp"
#include "errors.hpp"

namespace glfw
{
	Window::Window(int width, int height, const char* title):
		mWindow(glfwCreateWindow(width, height, title, nullptr, nullptr), &glfwDestroyWindow)
	{
		if (mWindow == nullptr)
		{
			LibraryError::checkLastError();
		}
	}

	void Window::process_events()
	{
		glfwPollEvents();
	}

	void Window::make_context_current() const
	{
		glfwMakeContextCurrent(mWindow.get());
		LibraryError::checkLastError();
	}

	bool Window::should_close() const
	{
		const auto closeFlag = glfwWindowShouldClose(mWindow.get());
		LibraryError::checkLastError();
		return closeFlag;
	}

	void Window::swap_buffers() const
	{
		glfwSwapBuffers(mWindow.get());
		LibraryError::checkLastError();
	}
}

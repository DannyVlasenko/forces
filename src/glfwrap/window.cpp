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

	Window::operator GLFWwindow*() const noexcept
	{
		return mWindow.get();
	}

	void Window::set_swap_interval(int screen_updates) const
	{
		makeContextCurrent();
		glfwSwapInterval(screen_updates);
	}

	void Window::makeContextCurrent() const
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

	void Window::swapBuffers() const
	{
		glfwSwapBuffers(mWindow.get());
		LibraryError::checkLastError();
	}

	glm::vec2 Window::size() const
	{
		int width, height;
		glfwGetWindowSize(mWindow.get(), &width, &height);
		LibraryError::checkLastError();
		return { width,height };
	}

	glm::vec2 Window::framebuffer_size() const
	{
		int width, height;
		glfwGetFramebufferSize(mWindow.get(), &width, &height);
		LibraryError::checkLastError();
		return { width,height };
	}

    bool Window::is_key_pressed(int key_code) const
    {
		const auto keyState = glfwGetKey(mWindow.get(), key_code);
		LibraryError::checkLastError();
		return keyState == GLFW_PRESS;
    }

	bool Window::mouse_button_pressed(int button) const
	{
		const auto buttonState = glfwGetMouseButton(mWindow.get(), button);
		LibraryError::checkLastError();
		return buttonState == GLFW_PRESS;
	}

	void Window::enable_raw_cursor(bool enable) const
	{
		if (glfwRawMouseMotionSupported()) {
			LibraryError::checkLastError();
			glfwSetInputMode(mWindow.get(), GLFW_RAW_MOUSE_MOTION, enable ? GLFW_TRUE : GLFW_FALSE);
			LibraryError::checkLastError();
		}
		LibraryError::checkLastError();
	}

	void Window::disable_cursor(bool disable) const
	{
		glfwSetInputMode(mWindow.get(), GLFW_CURSOR, disable ? GLFW_CURSOR_DISABLED : GLFW_CURSOR_NORMAL);
		LibraryError::checkLastError();
	}

	glm::vec2 Window::cursor_position() const
	{
		double x, y;
		glfwGetCursorPos(mWindow.get(), &x, &y);
		return { x, y };
	}
}

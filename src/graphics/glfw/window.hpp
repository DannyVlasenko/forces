#ifndef GLFW_WINDOW_HPP
#define GLFW_WINDOW_HPP

#include <memory>

#include "GLFW/glfw3.h"


namespace glfw
{
	class Window
	{
	public:
		Window(int width, int height, const char* title);

		static void process_events();

		void make_context_current() const;

		[[nodiscard]]
		bool should_close() const;

		void swap_buffers() const;

	private:
		std::unique_ptr<GLFWwindow, decltype(&glfwDestroyWindow)> mWindow;
	};
}
#endif // GLFW_WINDOW_HPP

#ifndef GLFW_WINDOW_HPP
#define GLFW_WINDOW_HPP

#include <memory>
#include <vec2.hpp>

#include "engine/render/window.hpp"
#include "GLFW/glfw3.h"


namespace glfw
{
	class Window : public forces::Window
	{
	public:
		Window(int width, int height, const char* title);

		static void process_events();

		operator GLFWwindow* () const noexcept;

		void set_swap_interval(int screen_updates) const;

		void makeContextCurrent() const override;

		[[nodiscard]]
		bool should_close() const;

		void swapBuffers() const override;

		[[nodiscard]]
		glm::vec2 size() const;

		[[nodiscard]]
		glm::vec2 framebuffer_size() const;

        [[nodiscard]]
	    bool is_key_pressed(int key_code) const;

		[[nodiscard]]
		bool mouse_button_pressed(int button) const;

		void enable_raw_cursor(bool enable) const;

		void disable_cursor(bool disable) const;

		[[nodiscard]]
		glm::vec2 cursor_position() const;

	private:
		std::unique_ptr<GLFWwindow, decltype(&glfwDestroyWindow)> mWindow;
	};
}
#endif // GLFW_WINDOW_HPP

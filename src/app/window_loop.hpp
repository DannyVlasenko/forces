#ifndef FORCES_WINDOW_LOOP_H
#define FORCES_WINDOW_LOOP_H

#include "glfw/window.hpp"

namespace forces
{
	class WindowLoop
	{
	public:
		explicit WindowLoop(const glfw::Window &window):
			mWindow(window)
		{}

		template <typename... Fn> requires (std::is_invocable_v<Fn> && ...)
		void run(const Fn&... tasks) const
		{
			while (!mWindow.should_close()) {
				glfw::Window::process_events();
				(tasks(), ...);
				mWindow.swap_buffers();
			}
		}

	private:
		const glfw::Window& mWindow;
	};
}

#endif // FORCES_WINDOW_LOOP_H

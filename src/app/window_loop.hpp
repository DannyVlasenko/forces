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

		void run(const auto&... tasks) const
		{
			while (!mWindow.should_close()) {
				(tasks(), ...);
				mWindow.swap_buffers();
				glfw::Window::process_events();
			}
		}

	private:
		const glfw::Window& mWindow;
	};
}

#endif // FORCES_WINDOW_LOOP_H

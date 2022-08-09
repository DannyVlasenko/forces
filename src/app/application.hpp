#ifndef FORCES_APPLICATION_HPP
#define FORCES_APPLICATION_HPP

#include "opengl/glcall.hpp"
#include "glfw/library.hpp"
#include "window_loop.h"

namespace forces
{

	class Application : glfw::EnableLibrary
	{
	public:
		Application();

		[[nodiscard]]
		const char* gl_version() const noexcept;

		void run() const;

	private:
		const glfw::Window mMainWindow{ 800, 600, "forces" };
		const WindowLoop mMainLoop;
	};

}
#endif // FORCES_APPLICATION_HPP

#ifndef FORCES_APPLICATION_HPP
#define FORCES_APPLICATION_HPP

#include "opengl/glcall.hpp"
#include "glfw/library.hpp"
#include "window_loop.hpp"

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
		const glfw::Window mMainWindow{ 1920, 1080, "forces" };
		const WindowLoop mMainLoop;
	};

}
#endif // FORCES_APPLICATION_HPP

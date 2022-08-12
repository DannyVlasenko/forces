#include <iostream>

#include "application.hpp"
#include "glfw/errors.hpp"
#include "opengl/glerrors.hpp"

#ifdef _WIN32
extern "C"
{
	__declspec(dllexport) unsigned long NvOptimusEnablement = 0x00000001;
}
#endif

int main()
{
	try
	{
		forces::Application app{};
		std::cout << app.gl_version() << std::endl;
		app.run();
		return EXIT_SUCCESS;
	}
	catch (glfw::LibraryError& glfw_err)
	{
		std::cout << "GLFW library error: " << glfw_err.what() << std::endl;
		return EXIT_FAILURE;
	}
	catch (opengl::OpenGLError &opengl_err)
	{
		std::cout << "Graphics library error: " << opengl_err.what() << std::endl;
		return EXIT_FAILURE;
	}
	catch (std::exception& e)
	{
		std::cout << "General application error: " << e.what() << std::endl;
		return EXIT_FAILURE;
	}
	catch (...)
	{
		std::cout << "Unknown application error occurred." << std::endl;
		return EXIT_FAILURE;
	}
}

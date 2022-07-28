#include "library.hpp"

#include "GLFW/glfw3.h"
#include "errors.hpp"

namespace glfw
{
	Library::Library()
	{
		if (!glfwInit())
		{
			LibraryError::checkLastError();
		}
	}

	Library::~Library()
	{
		glfwTerminate();
	}
}

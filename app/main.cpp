#include <iostream>
#include <memory>
#include <string>

#include "GL/glew.h"
#include "GLFW/glfw3.h"

namespace glfw
{
	class LibraryError : public std::runtime_error
	{
	public:
		using std::runtime_error::runtime_error;

		static void checkLastError()
		{
			const char* error;
			if (glfwGetError(&error) != GLFW_NO_ERROR)
			{
				throw LibraryError(error);
			}
		}
	};

	class Library
	{
	public:
		Library()
		{
			if (!glfwInit())
			{
				LibraryError::checkLastError();
			}
		}

		Library(const Library& other) = delete;
		Library(Library&& other) noexcept = delete;
		Library& operator=(const Library& other) = delete;
		Library& operator=(Library&& other) noexcept = delete;

		~Library()
		{
			glfwTerminate();
		}
	};

	class EnableLibrary
	{
		[[maybe_unused]]
		[[no_unique_address]]
		Library mGlfwLib{};
	};

	class Window
	{
	public:
		Window(int width, int height, const char* title):
			mWindow(glfwCreateWindow(width, height, title, nullptr, nullptr), &glfwDestroyWindow)
		{
			if (mWindow == nullptr)
			{
				LibraryError::checkLastError();
			}
		}

		void make_context_current() const
		{
			glfwMakeContextCurrent(mWindow.get());
			LibraryError::checkLastError();
		}

		[[nodiscard]]
		bool should_close() const
		{
			const auto closeFlag = glfwWindowShouldClose(mWindow.get());
			LibraryError::checkLastError();
			return closeFlag;
		}

		void swap_buffers() const
		{
			glfwSwapBuffers(mWindow.get());
			LibraryError::checkLastError();
		}

	private:
		std::unique_ptr<GLFWwindow, decltype(&glfwDestroyWindow)> mWindow;
	};
}

namespace gl
{
	class OpenGLError : public std::runtime_error
	{
	public:
		using std::runtime_error::runtime_error;

		static const char *error_msg_from_code(GLenum err) noexcept
		{
			switch (err)
			{
				case GL_NO_ERROR:
					return "No error has been recorded.The value of this symbolic constant is guaranteed to be 0.";

				case GL_INVALID_ENUM:
					return "An unacceptable value is specified for an enumerated argument.The offending command is ignored and has no other side effect than to set the error flag.";

				case GL_INVALID_VALUE:
					return "A numeric argument is out of range.The offending command is ignored and has no other side effect than to set the error flag.";

				case GL_INVALID_OPERATION:
					return "The specified operation is not allowed in the current state.The offending command is ignored and has no other side effect than to set the error flag.";

				case GL_INVALID_FRAMEBUFFER_OPERATION:
					return "The framebuffer object is not complete.The offending command is ignored and has no other side effect than to set the error flag.";

				case GL_OUT_OF_MEMORY:
					return "There is not enough memory left to execute the command.The state of the GL is undefined, except for the state of the error flags, after this error is recorded.";

				case GL_STACK_UNDERFLOW:
					return "An attempt has been made to perform an operation that would cause an internal stack to underflow.";

				case GL_STACK_OVERFLOW:
					return "An attempt has been made to perform an operation that would cause an internal stack to overflow.";
				default:
					return "Unspecified error.";
			}
		}
		
		static void clear()
		{
			while (glGetError() != GL_NO_ERROR);
		}

		static void check()
		{
			const auto err = glGetError();
			if (err != GL_NO_ERROR)
			{
				const auto* msg = error_msg_from_code(err);
#if !_DEBUG
				throw OpenGLError(msg);
#else
				__debugbreak();
#endif
			}
		}
	};
}

#define GLCall(x) \
	gl::OpenGLError::clear(); \
	x; \
	gl::OpenGLError::check()

static GLuint compile_shader(GLuint type, const char *source)
{
	GLCall(const auto id = glCreateShader(type));
	GLCall(glShaderSource(id, 1, &source, nullptr));
	GLCall(glCompileShader(id));

	int result;
	GLCall(glGetShaderiv(id, GL_COMPILE_STATUS, &result));
	if (result == GL_FALSE)
	{
		int length;
		GLCall(glGetShaderiv(id, GL_INFO_LOG_LENGTH, &length));
		std::string error(length, '\0');
		GLCall(glGetShaderInfoLog(id, error.length(), &length, error.data()));
		throw std::runtime_error("Shader compilation error: " + error);
	}

	return id;
}

static GLuint create_shader(const char *vertexShader, const char *fragmentShader)
{
	GLCall(auto program = glCreateProgram());
	auto vs = compile_shader(GL_VERTEX_SHADER, vertexShader);
	auto fs = compile_shader(GL_FRAGMENT_SHADER, fragmentShader);
	GLCall(glAttachShader(program, vs));
	GLCall(glAttachShader(program, fs));
	GLCall(glLinkProgram(program));
	GLCall(glValidateProgram(program));

	GLCall(glDeleteShader(vs));
	GLCall(glDeleteShader(fs));
	return program;
}

const char* VertexShader = R"--(
#version 330

layout(location = 0) in vec4 position;

void main()
{
	gl_Position = position;
}
)--";

const char* FragmentShader = R"--(
#version 330

layout(location = 0) out vec4 color;

void main()
{
	color = vec4(0.2, 0.3, 0.8, 1.0);
}
)--";

class Application : glfw::EnableLibrary
{
public:
	Application()
	{
		mMainWindow.make_context_current();
		if (glewInit() != GLEW_OK)
		{
			throw std::runtime_error("GLEW init error.");
		}
	}

	[[nodiscard]]
	const char * gl_version() const noexcept
	{
		return reinterpret_cast<const char *>(glGetString(GL_VERSION));
	}

	void run() const
	{			
		constexpr float positions[] = {
			-0.5f, -0.5f,
			 0.5f, -0.5f,
			 0.5f,  0.5f,
			-0.5f,  0.5f
		};

		constexpr GLuint indices[] = {
			0, 1, 2,
			2, 3, 0
		};

		GLuint buffer;
		GLCall(glGenBuffers(1, &buffer));
		GLCall(glBindBuffer(GL_ARRAY_BUFFER, buffer));
		GLCall(glBufferData(GL_ARRAY_BUFFER, sizeof(positions), positions, GL_STATIC_DRAW));
		GLCall(glEnableVertexAttribArray(0));
		GLCall(glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, sizeof(float) * 2, 0));
		GLCall(glBindBuffer(GL_ARRAY_BUFFER, 0));

		GLuint ibo;
		GLCall(glGenBuffers(1, &ibo));
		GLCall(glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ibo));
		GLCall(glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW));

		const auto shader = create_shader(VertexShader, FragmentShader);
		GLCall(glUseProgram(shader));
		
		while (!mMainWindow.should_close())
		{
			GLCall(glClear(GL_COLOR_BUFFER_BIT));
			GLCall(glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, nullptr));
			mMainWindow.swap_buffers();
			glfwPollEvents();
		}

		GLCall(glDeleteProgram(shader));
	}

private:
	const glfw::Window mMainWindow{ 800, 600, "forces" };
};

extern "C"
{
	__declspec(dllexport) unsigned long NvOptimusEnablement = 0x00000001;
}

int main()
{
	try
	{
		Application app{};
		std::cout << app.gl_version() << std::endl;
		app.run();
		return EXIT_SUCCESS;
	}
	catch (glfw::LibraryError& glfw_err)
	{
		std::cout << "GLFW library error: " << glfw_err.what() << std::endl;
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

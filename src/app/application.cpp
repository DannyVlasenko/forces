#include "application.hpp"

#include "opengl/buffer.hpp"
#include "opengl/vertex_array.hpp"

static GLuint compile_shader(GLuint type, const char* source)
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
		GLCall(glGetShaderInfoLog(id, static_cast<GLsizei>(error.length()), &length, error.data()));
		throw std::runtime_error("Shader compilation error: " + error);
	}

	return id;
}

static GLuint create_shader(const char* vertexShader, const char* fragmentShader)
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

uniform vec4 u_Color;

void main()
{
	color = u_Color;
}
)--";

namespace forces
{
	Application::Application() :
		mMainLoop(mMainWindow)
	{
		mMainWindow.make_context_current();
		if (glewInit() != GLEW_OK)
		{
			throw std::runtime_error("GLEW init error.");
		}
	}

	const char* Application::gl_version() const noexcept
	{
		return reinterpret_cast<const char*>(glGetString(GL_VERSION));
	}

	void Application::run() const
	{
		mMainWindow.set_swap_interval(1);

		constexpr float positions[] = {
			-0.5f, -0.5f,
			0.5f, -0.5f,
			0.5f, 0.5f,
			-0.5f, 0.5f
		};

		constexpr GLuint indices[] = {
			0, 1, 2,
			2, 3, 0
		};

		opengl::VertexArray va;
		opengl::VertexBuffer vb(positions);
		opengl::VertexBufferLayout layout;
		layout.push<float>(2);
		va.add_buffer(vb, layout);		

		opengl::IndexBuffer ib(indices);

		const auto shader = create_shader(VertexShader, FragmentShader);
		GLCall(glUseProgram(shader));
		GLCall(const auto location = glGetUniformLocation(shader, "u_Color"));
		GLCall(glUniform4f(location, 0.2f, 0.3f, 0.8f, 1.0f));

		GLCall(glBindVertexArray(0));
		GLCall(glUseProgram(0));
		GLCall(glBindBuffer(GL_ARRAY_BUFFER, 0));
		GLCall(glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0));

		float r = 0.0f;
		float increment = 0.05f;

		mMainLoop.run([&]
		{
			GLCall(glClear(GL_COLOR_BUFFER_BIT));

			GLCall(glUseProgram(shader));
			GLCall(glUniform4f(location, r, 0.3f, 0.8f, 1.0f));
			va.bind();
			GLCall(glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, nullptr));

			if (r < 0.0f || r > 1.0f)
			{
				increment = -increment;
			}
			r += increment;
		});

		GLCall(glDeleteProgram(shader));
	}
}

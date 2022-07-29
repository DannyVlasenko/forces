#include "application.hpp"

#include "opengl/buffer.hpp"

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
		GLCall(glGetShaderInfoLog(id, error.length(), &length, error.data()));
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

Application::Application()
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

	GLuint vao;
	GLCall(glGenVertexArrays(1, &vao));
	GLCall(glBindVertexArray(vao));

	opengl::VertexBuffer vb(positions);

	GLCall(glEnableVertexAttribArray(0));
	GLCall(glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, sizeof(float) * 2, 0));
	
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

	while (!mMainWindow.should_close())
	{
		GLCall(glClear(GL_COLOR_BUFFER_BIT));

		GLCall(glUseProgram(shader));
		GLCall(glUniform4f(location, r, 0.3f, 0.8f, 1.0f));
		GLCall(glBindVertexArray(vao));
		GLCall(glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, nullptr));

		if (r < 0.0f || r > 1.0f)
		{
			increment = -increment;
		}
		r += increment;

		mMainWindow.swap_buffers();
		glfwPollEvents();
	}

	GLCall(glDeleteProgram(shader));
}

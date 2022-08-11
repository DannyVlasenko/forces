#include "application.hpp"

#include "opengl/buffer.hpp"
#include "opengl/shader.hpp"
#include "opengl/vertex_array.hpp"
#include "opengl/renderer.hpp"

#include "glm.hpp"
#include "gtc/matrix_transform.hpp"

opengl::ShaderSource VertexSrc
{
	.type = GL_VERTEX_SHADER,
	.code = 
R"--(
#version 330

layout(location = 0) in vec4 position;

uniform mat4 u_MVP;

void main()
{
	gl_Position = u_MVP * position;
}
)--"
};

opengl::ShaderSource FragmentSrc
{
	.type = GL_FRAGMENT_SHADER,
	.code =
R"--(
#version 330

layout(location = 0) out vec4 color;

uniform vec4 u_Color;

void main()
{
	color = u_Color;
}
)--"
};

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

		glm::mat4 proj = glm::ortho(-2.0f, 2.0f, -1.5f, 1.5f, -1.0f, 1.0f);

		opengl::Program colorProgram{ opengl::Shader{ VertexSrc }, opengl::Shader{ FragmentSrc } };
		colorProgram.set_uniform_mat4("u_MVP", proj);
		colorProgram.set_uniform_4f("u_Color", 0.2f, 0.3f, 0.8f, 1.0f);

		opengl::Program::unbind();
		opengl::VertexArray::unbind();
		vb.unbind();

		opengl::Renderer renderer;

		float r = 0.0f;
		float increment = 0.05f;

		mMainLoop.run([&]
		{
			GLCall(glClear(GL_COLOR_BUFFER_BIT));

			colorProgram.bind();
			colorProgram.set_uniform_4f("u_Color", r, 0.3f, 0.8f, 1.0f);

			renderer.draw(va, ib, colorProgram);

			if (r < 0.0f || r > 1.0f)
			{
				increment = -increment;
			}
			r += increment;
		});
	}
}

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
#ifdef WIN32
		if (glewInit() != GLEW_OK)
		{
			throw std::runtime_error("GLEW init error.");
		}
#endif
	}

	const char* Application::gl_version() const noexcept
	{
		return reinterpret_cast<const char*>(glGetString(GL_VERSION));
	}

	void Application::run() const
	{
		mMainWindow.set_swap_interval(1);

		constexpr float positions[] = {
			-25.f, -25.f,
			25.f, -25.f,
			25.f, 25.f,
			-25.f, 25.f
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

		auto wndSize = mMainWindow.size();
		//glm::mat4 proj = glm::ortho(0.f, wndSize.x, 0.f, wndSize.y, -1.0f, 1.0f);

		auto proj = glm::perspective(glm::radians(60.f), wndSize.x / wndSize.y, 0.1f, 200.f);
		auto view = glm::lookAt(glm::vec3(0.f, 0.f, -100.f),
								glm::vec3(0,0,0),
								glm::vec3(0,1,0));
		auto model = glm::mat4(1.f);
		model = glm::rotate(model, glm::radians(30.f), glm::vec3(0.f, 1.f, 0.f));

		opengl::Program colorProgram{ opengl::Shader{ VertexSrc }, opengl::Shader{ FragmentSrc } };
		colorProgram.set_uniform_mat4("u_MVP", proj * view * model);
		colorProgram.set_uniform_4f("u_Color", 0.2f, 0.3f, 0.8f, 1.0f);

		opengl::Program::unbind();
		opengl::VertexArray::unbind();
		vb.unbind();

		opengl::Renderer renderer;

		float r = 0.0f;
		float increment = 0.05f;

		float grad = 0;

		mMainLoop.run([&]
		{
			GLCall(glClear(GL_COLOR_BUFFER_BIT));
			colorProgram.set_uniform_mat4("u_MVP", proj * view * model);
			colorProgram.set_uniform_4f("u_Color", r, 0.3f, 0.8f, 1.0f);

			renderer.draw(va, ib, colorProgram);
			auto y = 100 * glm::sin(glm::radians(grad));
			auto z = 100 * glm::cos(glm::radians(grad));
			grad += 0.5f;
			if (grad >= 360.f)
			{
				grad = 0.0f;
			}

			view = glm::lookAt(glm::vec3(0.f, y, z),
				glm::vec3(0, 0, 0),
				glm::vec3(0, 1, 0));

			if (r < 0.0f || r > 1.0f)
			{
				increment = -increment;
			}
			r += increment;
		});
	}
}

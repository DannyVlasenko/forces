#include "application.hpp"

#include "opengl/buffer.hpp"
#include "opengl/shader.hpp"
#include "opengl/vertex_array.hpp"
#include "opengl/renderer.hpp"

#include "glm.hpp"
#include "imgui.h"
#include "imgui_impl_glfw.h"
#include "imgui_impl_opengl3.h"
#include "gtc/matrix_transform.hpp"
#include "opengl/camera.hpp"

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
#ifdef _WIN32
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

		constexpr GLuint indicesFront[] = {
			0, 1, 2,
			2, 3, 0
		};

		constexpr GLuint indicesBack[] = {
			2, 1, 0,
			0, 3, 2
		};

		opengl::VertexArray va;
		opengl::VertexBuffer vb(positions);
		opengl::VertexBufferLayout layout;
		layout.push<float>(2);
		va.add_buffer(vb, layout);		

		opengl::IndexBuffer ibFront(indicesFront);
		opengl::IndexBuffer ibBack(indicesBack);

		auto wndSize = mMainWindow.size();
		//glm::mat4 proj = glm::ortho(0.f, wndSize.x, 0.f, wndSize.y, -1.0f, 1.0f);

		auto proj = glm::perspective(glm::radians(60.f), wndSize.x / wndSize.y, 0.1f, 200.f);

		opengl::Camera camera;
		camera.look_at() = glm::vec3(0.f, 0.f, -100.f);

		auto model = glm::mat4(1.f);
		model = glm::rotate(model, glm::radians(30.f), glm::vec3(0.f, 1.f, 0.f));

		opengl::Program colorProgram{ opengl::Shader{ VertexSrc }, opengl::Shader{ FragmentSrc } };
		colorProgram.set_uniform_mat4("u_MVP", proj * camera.view_matrix() * model);
		colorProgram.set_uniform_4f("u_Color", 0.2f, 0.3f, 0.8f, 1.0f);

		opengl::Program::unbind();
		opengl::VertexArray::unbind();
		vb.unbind();

		opengl::Renderer renderer;

		float r = 0.0f;
		float increment = 0.05f;

		GLCall(glEnable(GL_MULTISAMPLE));		
		GLCall(glEnable(GL_CULL_FACE));
		GLCall(glFrontFace(GL_CCW));
		GLCall(glCullFace(GL_BACK));

		ImGui::CreateContext();
		ImGui::StyleColorsDark();
		ImGui_ImplGlfw_InitForOpenGL(mMainWindow, true);
		ImGui_ImplOpenGL3_Init("#version 330");
		ImVec4 clear_color = ImVec4(0.45f, 0.55f, 0.60f, 1.00f);
		mMainLoop.run([&]
		{
			int display_w, display_h;
			glfwGetFramebufferSize(mMainWindow, &display_w, &display_h);
			glViewport(0, 0, display_w, display_h);
			GLCall(glClearColor(clear_color.x * clear_color.w, clear_color.y * clear_color.w, clear_color.z * clear_color.w, clear_color.w));
			GLCall(glClear(GL_COLOR_BUFFER_BIT));
			{
				colorProgram.set_uniform_mat4("u_MVP", proj * camera.view_matrix() * model);

				colorProgram.set_uniform_4f("u_Color", r, 0.3f, 0.8f, 1.0f);
				renderer.draw(va, ibFront, colorProgram);

				colorProgram.set_uniform_4f("u_Color", 0.2f, r, 0.3f, 1.0f);
				renderer.draw(va, ibBack, colorProgram);

				if (r < 0.0f || r > 1.0f)
				{
					increment = -increment;
				}
				r += increment;
			}
			{
				ImGui_ImplOpenGL3_NewFrame();
				ImGui_ImplGlfw_NewFrame();
				ImGui::NewFrame();
				ImGui::Begin("Camera");                          // Create a window called "Hello, world!" and append into it.


				static float f = 0.0f;
				static int counter = 0;
				ImGui::DragFloat3("Translation", &camera.translation()[0]);           // Edit 1 float using a slider from 0.0f to 1.0f
				ImGui::DragFloat3("Look At", &camera.look_at()[0]);           // Edit 1 float using a slider from 0.0f to 1.0f
				ImGui::DragFloat3("Up", &camera.up()[0]);           // Edit 1 float using a slider from 0.0f to 1.0f
				ImGui::ColorEdit3("clear color", (float*)&clear_color); // Edit 3 floats representing a color

				if (ImGui::Button("Button"))                            // Buttons return true when clicked (most widgets return true when edited/activated)
					counter++;
				ImGui::SameLine();
				ImGui::Text("counter = %d", counter);

				ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);
				ImGui::End();

				ImGui::Render();
				ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());
			}
		});

		ImGui_ImplGlfw_Shutdown();
		ImGui::DestroyContext();
	}
}

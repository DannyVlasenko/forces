#include "application.hpp"

#include "app_ui.hpp"
#include "opengl/buffer.hpp"
#include "opengl/shader.hpp"
#include "opengl/vertex_array.hpp"
#include "opengl/renderer.hpp"

#include "glm.hpp"
#include "imgui.h"
#include "controllers/camera_move_controller.hpp"
#include "gtc/matrix_transform.hpp"
#include "view_models/camera_view_model.hpp"
#include "view_models/global_light_view_model.hpp"

opengl::ShaderSource VertexSrc
{
	.type = GL_VERTEX_SHADER,
	.code =
	R"--(
#version 330

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 inNormal;

uniform mat4 viewProjection;
uniform mat4 model;
uniform mat3 normalModel;

out vec3 normal;
out vec3 fragPos;

void main()
{	
	fragPos = vec3(model * vec4(position, 1.0));
	gl_Position = viewProjection * vec4(fragPos, 1.0);
	normal = normalModel * inNormal;
}
)--"
};

opengl::ShaderSource FragmentSrc
{
	.type = GL_FRAGMENT_SHADER,
	.code =
	R"--(
#version 330

in vec3 normal;
in vec3 fragPos;

layout(location = 0) out vec4 color;

uniform vec3 objectColor;
uniform vec3 ambientLightColor;
uniform vec3 diffuseLightPosition;  
uniform vec3 diffuseLightColor;  

void main()
{
	vec3 norm = normalize(normal);
	vec3 lightDir = normalize(diffuseLightPosition - fragPos);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuseColor = diff * diffuseLightColor;
	vec3 resColor = (diffuseColor + ambientLightColor) * objectColor;
	color = vec4(resColor, 1.0);
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
		//Camera
		models::Camera camera;
		camera.position() = glm::vec3(0.0f, 0.0f, 0.0f);
		//camera.look_at() = glm::vec3(0.0f, 0.0f, -1.0f);
		camera.far() = 20.f;
		view_models::CameraViewModel cameraViewModel{camera, mMainWindow};
		controllers::CameraMoveController cameraMoveController{ mMainWindow, camera };

		//Material
		opengl::Program colorProgram{opengl::Shader{VertexSrc}, opengl::Shader{FragmentSrc}};
		colorProgram.set_uniform_mat4("viewProjection", camera.view_projection());
		colorProgram.set_uniform_3f("objectColor", 0.2f, 0.3f, 0.8f);
		view_models::GlobalLightViewModel globalLightViewModel{colorProgram};

		//UI
		views::AppUI ui{mMainWindow};
		ui.add_view(std::make_unique<views::CameraView>(cameraViewModel));
		ui.add_view(std::make_unique<views::GlobalLightView>(globalLightViewModel));

		//Model
		auto model = glm::mat4(1.f);
		model = translate(model, glm::vec3(0.0f, 0.0f, 5.0f));
		model = rotate(model, glm::radians(15.f), glm::vec3(0.f, 1.f, 0.f));
		colorProgram.set_uniform_mat4("model", model);
		auto normalModel = transpose(inverse(glm::mat3(model)));
		colorProgram.set_uniform_mat3("normalModel", normalModel);

		float vertices[] = {
			//0
			-0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
			 0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
			 0.5f,  0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
			-0.5f,  0.5f, 0.5f, 0.0f, 0.0f, 1.0f,

			//4
			-0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
			 0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
			 0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
			-0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f,

			//8
			-0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f,
			-0.5f, -0.5f,  0.5f, -1.0f, 0.0f, 0.0f,
			-0.5f,  0.5f,  0.5f, -1.0f, 0.0f, 0.0f,
			-0.5f,  0.5f, -0.5f, -1.0f, 0.0f, 0.0f,

			//12
			 0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f,
			 0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 0.0f,
			 0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 0.0f,
			 0.5f,  0.5f, -0.5f,  1.0f, 0.0f, 0.0f,

			//16
			-0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f,
			 0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f,
			 0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f,
			-0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f,

			//20
			-0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
			 0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
			 0.5f, 0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
			-0.5f, 0.5f,  0.5f, 0.0f, 1.0f, 0.0f
		};

		GLuint indices[] =
		{
			 0,  1,  2,  0,  2,  3,
			 4,  7,  6,  4,  6,  5,
			 8,  9, 10,  8, 10, 11,
			12, 15, 14, 12, 14, 13,
			16, 17, 18, 16, 18, 19,
			20, 23, 22, 20, 22, 21
		};

		opengl::VertexArray va;
		opengl::VertexBuffer vb(vertices);
		opengl::VertexBufferLayout layout;
		layout.push<float>(3);
		layout.push<float>(3);
		va.add_buffer(vb, layout);

		opengl::IndexBuffer ib(indices);
		opengl::Program::unbind();
		opengl::VertexArray::unbind();
		vb.unbind();

		opengl::Renderer renderer;

		float r = 0.0f;
		float increment = 0.05f;

		GLCall(glEnable(GL_MULTISAMPLE));
		GLCall(glEnable(GL_CULL_FACE));
		GLCall(glEnable(GL_DEPTH_TEST));

		mMainLoop.run([&]
		{
			cameraViewModel.update();
			globalLightViewModel.diffuse_position() = cameraViewModel.position();
			globalLightViewModel.update();
			cameraMoveController.update();
			{
				colorProgram.set_uniform_mat4("viewProjection", camera.view_projection());

				colorProgram.set_uniform_3f("objectColor", r, 0.3f, 0.8f);
				renderer.draw(va, ib, colorProgram);

				if (r < 0.0f || r > 1.0f)
				{
					increment = -increment;
				}
				r += increment;
			}
			ui.render();
		});
	}
}

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
#include "opengl/mesh.hpp"
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
uniform vec3 pointLightPosition;  
uniform vec3 pointLightColor;
uniform vec3 directedLightOrient;  
uniform vec3 directedLightColor;    

void main()
{
	vec3 norm = normalize(normal);

	vec3 pointLightDir = normalize(pointLightPosition - fragPos);
	float pointDiff = max(dot(norm, pointLightDir), 0.0);
	vec3 pointDiffuseColor = pointDiff * pointLightColor;

	float directedDiff = max(dot(norm, normalize(directedLightOrient)), 0.0);
	vec3 directedDiffuseColor = directedDiff * directedLightColor;

	vec3 resColor = (directedDiffuseColor + pointDiffuseColor + ambientLightColor) * objectColor;
	color = vec4(resColor, 1.0);
}
)--"
};

class SceneObject
{
public:
	SceneObject(const opengl::Mesh &mesh, const opengl::Program &program):
		mMesh(mesh),
		mProgram(program)
	{}

	void draw() 
	{
		mProgram.bind();
		mProgram.set_uniform("objectColor", mColor);
		auto model = glm::mat4(1.f);
		model = translate(model, mPosition);
		model = glm::scale(model, mScale);
		mProgram.set_uniform("model", model);
		auto normalModel = transpose(inverse(glm::mat3(model)));
		mProgram.set_uniform("normalModel", normalModel);
		mMesh.draw();
		mProgram.unbind();
	}

	glm::vec3& postion() noexcept
	{
		return mPosition;
	}

	glm::vec3& scale() noexcept
	{
		return mScale;
	}

	glm::vec3& color() noexcept
	{
		return mColor;
	}

private:
	const opengl::Mesh & mMesh;
	const opengl::Program & mProgram;
	glm::vec3 mColor{ 1.0f };
	glm::vec3 mPosition{ 0.0f };
	glm::vec3 mScale{ 1.0f };

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
		camera.far() = 20.f;
		view_models::CameraViewModel cameraViewModel{camera, mMainWindow};
		controllers::CameraMoveController cameraMoveController{ mMainWindow, camera };

		//Material
		opengl::Program colorProgram{opengl::Shader{VertexSrc}, opengl::Shader{FragmentSrc}};
		colorProgram.set_uniform("viewProjection", camera.view_projection());
		view_models::GlobalLightViewModel globalLightViewModel{colorProgram, camera};

		//UI
		views::AppUI ui{mMainWindow};
		ui.add_view(std::make_unique<views::CameraView>(cameraViewModel));
		ui.add_view(std::make_unique<views::GlobalLightView>(globalLightViewModel));

		//Model
		const auto meshes = opengl::load_from_file("sphere.obj");
		SceneObject sphere1{ meshes.at(0), colorProgram };
		sphere1.postion() += 1.0f;
		sphere1.scale() = glm::vec3{ 0.3f }; 
		sphere1.color() = glm::vec3{ 0.1f, 0.3f, 0.8f };

		SceneObject sphere2{ meshes.at(0), colorProgram };
		sphere2.postion() -= 1.0f;
		sphere2.scale() = glm::vec3{ 0.5f };
		sphere2.color() = glm::vec3{ 0.1f, 0.8f, 0.3f };

		GLCall(glEnable(GL_MULTISAMPLE));
		GLCall(glEnable(GL_CULL_FACE));
		GLCall(glEnable(GL_DEPTH_TEST));

		mMainLoop.run([&]
		{
			cameraViewModel.update();
			//globalLightViewModel.diffuse_position() = cameraViewModel.position();
			globalLightViewModel.update();
			cameraMoveController.update();
			{
				colorProgram.set_uniform("viewProjection", camera.view_projection());
				sphere1.draw();
				sphere2.draw();
			}
			ui.render();
		});
	}
}

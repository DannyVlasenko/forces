#include "application.hpp"

#include "app_ui.hpp"
#include "opengl/buffer.hpp"
#include "opengl/shader.hpp"
#include "opengl/vertex_array.hpp"
#include "opengl/renderer.hpp"

#include "glm.hpp"
#include "imgui.h"
#include "lighted_object.hpp"
#include "controllers/camera_move_controller.hpp"
#include "gtc/matrix_transform.hpp"
#include "opengl/mesh.hpp"
#include "view_models/camera_view_model.hpp"
#include "view_models/global_light_view_model.hpp"

opengl::ShaderSource FieldVertexSrc
{
	.type = GL_VERTEX_SHADER,
	.code =
	R"--(
#version 330

layout(location = 0) in vec3 position;

uniform vec3 body1Pos;
uniform float body1Mass;
uniform vec3 body2Pos;
uniform float body2Mass;	
uniform vec3 body3Pos;
uniform float body3Mass;	

out VS_OUT {
    vec3 potential;
} vs_out;

void main()
{
	float G = 6.6743e-11;
    vec3 direction1 = normalize(body1Pos - position);
    float dist1 = distance(body1Pos, position);
    vec3 pot1 = (G * body1Mass)/(dist1 * dist1) * direction1; 

    vec3 direction2 = normalize(body2Pos - position);
    float dist2 = distance(body2Pos, position);
	vec3 pot2 = (G * body2Mass)/(dist2 * dist2) * direction2;

    vec3 direction3 = normalize(body3Pos - position);
    float dist3 = distance(body3Pos, position);
	vec3 pot3 = (G * body3Mass)/(dist3 * dist3) * direction3;

    vs_out.potential = pot1 + pot2 + pot3; 
	gl_Position = vec4(position, 1.0);
}
)--"
};

opengl::ShaderSource FieldGeometrySrc
{
	.type = GL_GEOMETRY_SHADER,
	.code =
	R"--(
#version 330

layout (points) in;
layout (line_strip, max_vertices = 2) out;

in VS_OUT {
    vec3 potential;
} gs_in[];

uniform mat4 viewProjection;
uniform mat4 model;

void main()
{
	gl_Position = viewProjection * model * gl_in[0].gl_Position; 
    EmitVertex();

    gl_Position = viewProjection * model * vec4(vec3(gl_in[0].gl_Position) + gs_in[0].potential, 1.0);
    EmitVertex();
    
    EndPrimitive();
}
)--"
};

opengl::ShaderSource FieldFragmentSrc
{
	.type = GL_FRAGMENT_SHADER,
	.code =
	R"--(
#version 330

layout(location = 0) out vec4 color;

void main()
{
	color = vec4(1.0, 1.0, 1.0, 1.0);
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
		camera.position() = glm::vec3(0.0f, 0.0f, -20.0f);
		camera.far() = 200.f;
		view_models::CameraViewModel cameraViewModel{camera, mMainWindow};
		controllers::CameraMoveController cameraMoveController{ mMainWindow, camera };

		//Material
		models::LightProgram lightProgram;
		view_models::GlobalLightViewModel globalLightViewModel{lightProgram, camera};
		globalLightViewModel.directed_orientation() = glm::vec3(1.0f, 1.0f, -1.0f);

		//UI
		views::AppUI ui{mMainWindow};
		ui.add_view(std::make_unique<views::CameraView>(cameraViewModel));
		ui.add_view(std::make_unique<views::GlobalLightView>(globalLightViewModel));

		//Model
		const auto meshes = opengl::load_from_file("sphere.obj");
		models::LightedObject sphere1{ meshes.at(0), lightProgram };
		sphere1.postion() = glm::vec3(-5.0f, 0.0f, 0.0f);
		sphere1.scale() = glm::vec3{ 0.5f }; 
		sphere1.color() = glm::vec3{ 0.1f, 0.3f, 0.8f };

		models::LightedObject sphere2{ meshes.at(0), lightProgram };
		sphere2.postion() = glm::vec3(5.0f, 0.0f, 0.0f);
		sphere2.scale() = glm::vec3{ 0.3f };
		sphere2.color() = glm::vec3{ 0.1f, 0.8f, 0.3f };

		models::LightedObject sphere3{ meshes.at(0), lightProgram };
		sphere3.postion() = glm::vec3(0.0f, 5.0f, 0.0f);
		sphere3.scale() = glm::vec3{ 0.1f };
		sphere3.color() = glm::vec3{ 0.8f, 0.1f, 0.3f };

		opengl::Program fieldProgram{ opengl::Shader{FieldVertexSrc}, opengl::Shader{FieldGeometrySrc}, opengl::Shader{FieldFragmentSrc} };
		fieldProgram.set_uniform("viewProjection", camera.view_projection());
		fieldProgram.set_uniform("model", glm::mat4{1.0f});
		fieldProgram.set_uniform("body1Pos", sphere1.postion());
		fieldProgram.set_uniform("body1Mass", 5e10f);
		fieldProgram.set_uniform("body2Pos", sphere2.postion());
		fieldProgram.set_uniform("body2Mass", 3e10f);
		fieldProgram.set_uniform("body3Pos", sphere3.postion());
		fieldProgram.set_uniform("body3Mass", 1e10f);

		std::vector<opengl::VertexSimple> fieldVertices;
		std::vector<GLuint> indices;
		GLuint i = 0;
		for (auto x = -10; x <= 10; ++x)
		{
			for (auto y = -10; y <= 10; ++y)
			{
				for (auto z = -2; z <= 2; ++z)
				{
					const auto point = glm::vec3{ x, y, z };
					if (distance(point, sphere1.postion()) < 1.4f) continue;
					if (distance(point, sphere2.postion()) < 1.4f) continue;
					if (distance(point, sphere3.postion()) < 1.4f) continue;
					fieldVertices.push_back({ point });
					indices.push_back(++i);
				}
			}
		}

		opengl::Mesh<opengl::VertexSimple> gravityFieldMesh(fieldVertices, indices);

		GLCall(glEnable(GL_MULTISAMPLE));
		GLCall(glEnable(GL_CULL_FACE));
		GLCall(glEnable(GL_DEPTH_TEST));

		mMainLoop.run([&]
		{
			cameraViewModel.update();
			globalLightViewModel.update();
			cameraMoveController.update();
			{
				sphere1.draw(camera.view_projection());
				sphere2.draw(camera.view_projection());
				sphere3.draw(camera.view_projection());
				//fieldProgram.set_uniform("viewProjection", camera.view_projection());
				//fieldProgram.bind();
				//gravityFieldMesh.draw(GL_POINTS);
			}
			ui.render();
		});
	}
}

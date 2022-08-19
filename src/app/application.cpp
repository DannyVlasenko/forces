#include "application.hpp"

#include "app_ui.hpp"
#include "opengl/buffer.hpp"
#include "opengl/shader.hpp"
#include "opengl/vertex_array.hpp"
#include "opengl/renderer.hpp"

#include "glm.hpp"
#include "imgui.h"
#include "gtc/matrix_transform.hpp"
#include "camera.hpp"
#include "camera_view.hpp"
#include "global_light_view.hpp"

opengl::ShaderSource VertexSrc
{
	.type = GL_VERTEX_SHADER,
	.code = 
R"--(
#version 330

layout(location = 0) in vec4 position;

uniform mat4 viewProjection;
uniform mat4 model;

void main()
{
	gl_Position = viewProjection * model * position;
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

uniform vec3 objectColor;
uniform vec3 ambientLightColor;

void main()
{
	vec3 resColor = ambientLightColor * objectColor;
	color = vec4(resColor, 1.0);
}
)--"
};

class CameraViewModel final : public views::ICameraViewModel
{
public:
	CameraViewModel(models::Camera &camera, const glfw::Window &window):
		mCamera(camera),
		mWindow(window)
	{}

	glm::vec3& look_at() noexcept override { return mCamera.look_at(); }
	glm::vec3& translation() noexcept override { return mCamera.translation(); }
	glm::vec3& up() noexcept override { return mCamera.up(); }
	float& fov() noexcept override { return mCamera.fov(); }
	glm::vec2& viewport() noexcept override { return mCamera.viewport(); }
	float& near() noexcept override { return mCamera.near(); }
	float& far() noexcept override { return mCamera.far(); }
	bool& v_sync() noexcept override { return mVSync; }
	bool& viewport_match_window() noexcept override { return mViewportMatchWindow; }
	glm::vec3& clear_color() noexcept override { return mClearColor; }

	void update()
	{
		mWindow.set_swap_interval(mVSync);
		if (mViewportMatchWindow)
		{
			mCamera.viewport() = mWindow.framebuffer_size();
		}
		GLCall(glViewport(0, 0, mCamera.viewport().x, mCamera.viewport().y));
		GLCall(glClearColor(mClearColor.x, mClearColor.y, mClearColor.z, 1.0f));
		GLCall(glClear(GL_COLOR_BUFFER_BIT));
	}

private:
	bool mVSync{ true };
	bool mViewportMatchWindow{ true };
	glm::vec3 mClearColor{ 0.2f, 0.2f, 0.2f };
	models::Camera& mCamera;
	const glfw::Window& mWindow;
};

class GlobalLightViewModel final : public views::IGlobalLightViewModel
{
public:
	GlobalLightViewModel(opengl::Program &shader):
		mShader(shader)
	{}

	bool& ambient_light_enabled() noexcept override
	{
		return mAmbientEnabled;
	}

	glm::vec3& ambient_color() noexcept override
	{
		return mAmbientColor;
	}

	float& ambient_strength() noexcept override
	{
		return mAmbientStrength;
	}


	void update()
	{
		const auto resultColor = mAmbientEnabled ? mAmbientColor * mAmbientStrength : glm::vec3(0.0f);
		mShader.set_uniform_3f("ambientLightColor", resultColor.x, resultColor.y, resultColor.z);
	}

private:
	opengl::Program& mShader;
	bool mAmbientEnabled{ true };
	glm::vec3 mAmbientColor{ 1.0f };
	float mAmbientStrength{ 1.0f };
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
		camera.translation() = glm::vec3(-1.0f, 4.0f, 5.0f);
		camera.look_at() = glm::vec3(0.0f, 0.0f, -5.0f);
		camera.far() = 20.f;
		CameraViewModel cameraViewModel{ camera, mMainWindow };

		//Material
		opengl::Program colorProgram{ opengl::Shader{ VertexSrc }, opengl::Shader{ FragmentSrc } };
		colorProgram.set_uniform_mat4("viewProjection", camera.view_projection());
		colorProgram.set_uniform_3f("objectColor", 0.2f, 0.3f, 0.8f);
		colorProgram.set_uniform_3f("ambientLightColor", 1.0f, 1.0f, 1.0f);
		GlobalLightViewModel globalLightViewModel{ colorProgram };

		//UI
		views::AppUI ui{ mMainWindow };
		ui.add_view(std::make_unique<views::CameraView>(cameraViewModel));
		ui.add_view(std::make_unique<views::GlobalLightView>(globalLightViewModel));

		//Model
		auto model = glm::mat4(1.f);
		model = glm::translate(model, glm::vec3(0.0f, 0.0f, -5.0f));
		model = glm::rotate(model, glm::radians(30.f), glm::vec3(0.f, 1.f, 0.f));
		colorProgram.set_uniform_mat4("model", model);
				
		constexpr float positions[] = {
			-2.f, -2.f,
			 2.f, -2.f,
			 2.f,  2.f,
			-2.f,  2.f
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
				
		mMainLoop.run([&]
		{
			cameraViewModel.update();
			globalLightViewModel.update();
			{
				colorProgram.set_uniform_mat4("viewProjection", camera.view_projection());

				colorProgram.set_uniform_3f("objectColor", r, 0.3f, 0.8f);
				renderer.draw(va, ibFront, colorProgram);

				colorProgram.set_uniform_3f("objectColor", 0.2f, r, 0.3f);
				renderer.draw(va, ibBack, colorProgram);

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

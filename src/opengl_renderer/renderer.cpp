//opengl_renderer
#include "renderer.hpp"
#include "scene/lighted_object.hpp"

//engine
#include "engine/scene/node.hpp"
#include "engine/scene/mesh.hpp"
#include "engine/scene/scene.hpp"
#include "scene/light_sources.hpp"

//stl
#include <unordered_map>


namespace opengl
{
	glm::mat4 view_projection(const forces::Camera &camera, const glm::vec3& pos, const glm::vec3& front, const glm::vec3& up)
	{
		return glm::perspective(glm::radians(camera.fov()), camera.viewport().x / camera.viewport().y, camera.near(), camera.far())
			* glm::lookAt(pos, pos + front, up);
	}

	struct Renderer::Impl
	{
		Impl(forces::Window &window):window_(window){}

		forces::Window& window_;
		std::unordered_map<const forces::Mesh*, std::vector<Mesh<VertexNormal>>> meshes_;
		std::vector<LightedObject> objects_;
		AmbientLight ambient_light_{};
		std::vector<PointLight> pointLights_;
		std::vector<DirectedLight> directedLights_;
		std::unique_ptr<LightProgram> lightProgram_;
		forces::CameraNode* camera_{nullptr};

		void addNodeWithChildren(const std::variant<forces::ConcreteNode>& node, glm::vec3 parentTranslation)
		{
			std::visit(
				[&parentTranslation](const auto& nd)
				{
					addNodeWithChildren(nd, parentTranslation);
				}, node);
		}

		void addNodeWithChildren(const forces::MeshNode& node, glm::vec3 parentTranslation)
		{
			const auto thisTranslation = node.translation() + parentTranslation;
			auto* nodeMesh = node.mesh();
			if (nodeMesh != nullptr) 
			{
				if (!meshes_.contains(nodeMesh))
				{
					meshes_.insert_or_assign(nodeMesh, load_from_file(nodeMesh->path()));
				}
				for (const auto& mesh : meshes_.at(nodeMesh))
				{
					auto& obj = objects_.emplace_back(mesh, *lightProgram_);
					obj.postion() = thisTranslation;
					obj.color() = node.material()->color();
				}
			}
			for(const auto &child : node.children())
			{
				addNodeWithChildren(child, thisTranslation);
			}
		}

		void addNodeWithChildren(const forces::LightNode& node, glm::vec3 parentTranslation)
		{
			const auto thisTranslation = node.translation() + parentTranslation;
			pointLights_.push_back(PointLight{ thisTranslation, node.light().color() });

			for(const auto &child : node.children())
			{
				addNodeWithChildren(child, thisTranslation);
			}
		}

		void addNodeWithChildren(const forces::CameraNode& node, glm::vec3 parentTranslation)
		{
			const auto thisTranslation = node.translation() + parentTranslation;
			for(const auto &child : node.children())
			{
				addNodeWithChildren(child, thisTranslation);
			}
		}

		void addNodeWithChildren(const forces::EmptyNode& node, glm::vec3 parentTranslation)
		{
			const auto thisTranslation = node.translation() + parentTranslation;
			for(const auto &child : node.children())
			{
				addNodeWithChildren(child, thisTranslation);
			}
		}
	};

	Renderer::Renderer(forces::Window& window):
		pImpl_(std::make_unique<Impl>(window))
	{
		if (glewInit() != GLEW_OK)
		{
			throw std::runtime_error("GLEW init error.");
		}
		pImpl_->lightProgram_ = std::make_unique<LightProgram>();
	}

	void Renderer::render()
	{
		pImpl_->window_.makeContextCurrent();
		GLCall(glEnable(GL_MULTISAMPLE));
		GLCall(glEnable(GL_CULL_FACE));
		GLCall(glEnable(GL_DEPTH_TEST));
		GLCall(glClearColor(pImpl_->ambient_light_.Color.x, pImpl_->ambient_light_.Color.y, pImpl_->ambient_light_.Color.z, 1.0f));
		GLCall(glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT));

		if (pImpl_->camera_ == nullptr)
		{
			return;
		}

		GLCall(glViewport(0, 0, pImpl_->camera_->camera().viewport().x, pImpl_->camera_->camera().viewport().y));
		pImpl_->lightProgram_->setAmbientLightColor(pImpl_->ambient_light_.Color);
		if (!pImpl_->directedLights_.empty())
		{
			pImpl_->lightProgram_->setDirectedLightColor(pImpl_->directedLights_.front().Color);
			pImpl_->lightProgram_->setDirectedLightOrientation(pImpl_->directedLights_.front().Direction);
		}
		if (!pImpl_->pointLights_.empty())
		{
			pImpl_->lightProgram_->setPointLightColor(pImpl_->pointLights_.front().Color);
			pImpl_->lightProgram_->setPointLightPosition(pImpl_->pointLights_.front().Position);
		}
		for (const auto& obj : pImpl_->objects_)
		{

			obj.draw(view_projection(pImpl_->camera_->camera(), 
										pImpl_->camera_->translation(), 
										front(*pImpl_->camera_), 
										up(*pImpl_->camera_)));
		}
		pImpl_->window_.swapBuffers();
	}

	void Renderer::processScene(const forces::Scene& scene)
	{
		pImpl_->window_.makeContextCurrent();
		pImpl_->ambient_light_.Color = scene.ambientLight().color();
		pImpl_->directedLights_.clear();
		for (const auto& dl : scene.directedLights())
		{
			pImpl_->directedLights_.push_back(DirectedLight{ dl.direction(), dl.color() });
		}		
		pImpl_->objects_.clear();
		pImpl_->addNodeWithChildren(scene.rootNode(), glm::vec3{ 0.f, 0.f, 0.f });
	}
}

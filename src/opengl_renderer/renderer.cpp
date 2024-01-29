//opengl_renderer
#include "renderer.hpp"
#include "scene/lighted_object.hpp"

//engine
#include "engine/scene/node.hpp"
#include "engine/scene/mesh.hpp"

//stl
#include <unordered_map>

namespace opengl
{
	struct Renderer::Impl
	{
		std::unordered_map<forces::Mesh*, std::vector<Mesh<VertexNormal>>> meshes_;
		std::vector<LightedObject> objects_;
		std::unique_ptr<LightProgram> lightProgram_;
		forces::Camera* camera_;

		void addNodeWithChildren(const forces::Node& node, glm::vec3 parentTranslation)
		{
			const auto thisTranslation = node.translation() + parentTranslation;
			auto* nodeMesh = node.getMesh();
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
					obj.color() = { 0.2f, 0.8f, 0.2f };
				}
			}
			for(const auto &child : node.children())
			{
				addNodeWithChildren(child, thisTranslation);
			}
		}
	};

	Renderer::Renderer():
		pImpl_(std::make_unique<Impl>())
	{
		if (glewInit() != GLEW_OK)
		{
			throw std::runtime_error("GLEW init error.");
		}
		pImpl_->lightProgram_ = std::make_unique<LightProgram>();		
		pImpl_->lightProgram_->setAmbientLightColor({ 0.2f, 0.2, 0.2f });
		pImpl_->lightProgram_->setDirectedLightOrientation({ -1.f, -1.f, -1.f });
		pImpl_->lightProgram_->setDirectedLightColor({ 0.6f, 0.6f, 0.6f });
		pImpl_->lightProgram_->setPointLightColor({ 1.0f, 1.0f, 1.0f });
		pImpl_->lightProgram_->setPointLightPosition({ 1.0f, 5.0f, 1.0f });
	}

	void Renderer::setCurrentRootNode(const forces::Node& root)
	{
		pImpl_->objects_.clear();
		pImpl_->addNodeWithChildren(root, glm::vec3{ 0.f, 0.f, 0.f });
	}

	void Renderer::render()
	{
		GLCall(glEnable(GL_MULTISAMPLE));
		GLCall(glEnable(GL_CULL_FACE));
		GLCall(glEnable(GL_DEPTH_TEST));
		GLCall(glClearColor(0.3f, 0.3f, 0.3f, 1.0f));
		GLCall(glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT));

		if (pImpl_->camera_ == nullptr)
		{
			return;
		}

		GLCall(glViewport(0, 0, pImpl_->camera_->viewport().x, pImpl_->camera_->viewport().y));
		for (const auto& obj : pImpl_->objects_)
		{
			obj.draw(pImpl_->camera_->view_projection());
		}
	}

	void Renderer::setCamera(forces::Camera* camera) noexcept
	{
		pImpl_->camera_ = camera;
	}
}

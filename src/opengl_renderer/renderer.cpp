//opengl_renderer
#include "renderer.hpp"
#include "scene/camera.hpp"
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
		Camera camera_;

		void addNodeWithChildren(const forces::INode& node, glm::vec3 parentTranslation)
		{
			const auto thisTranslation = node.getTranslation() + parentTranslation;
			for (auto *nodeMesh : node.getMeshes())
			{
				if (!meshes_.contains(nodeMesh))
				{
					meshes_.insert_or_assign(nodeMesh, load_from_file(nodeMesh->path()));
				}
				for (const auto &mesh : meshes_.at(nodeMesh))
				{
					auto &obj = objects_.emplace_back(mesh, *lightProgram_);
					obj.postion() = thisTranslation;
				}
			}
			for(const auto &child : node.getChildren())
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
	}

	void Renderer::setCurrentRootNode(const forces::INode& root)
	{
		pImpl_->objects_.clear();
		pImpl_->addNodeWithChildren(root, glm::vec3{ 0.f, 0.f, 0.f });
	}

	void Renderer::render()
	{
		GLCall(glEnable(GL_MULTISAMPLE));
		GLCall(glEnable(GL_CULL_FACE));
		GLCall(glEnable(GL_DEPTH_TEST));
		GLCall(glClearColor(0.1f, 0.3f, 0.6f, 1.0f));
		GLCall(glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT));

		for (const auto& obj : pImpl_->objects_)
		{
			obj.draw(pImpl_->camera_.view_projection());
		}
	}
}

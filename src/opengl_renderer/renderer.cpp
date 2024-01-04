//opengl_renderer
#include "renderer.hpp"
#include "scene/camera.hpp"
#include "scene/lighted_object.hpp"

//engine
#include "engine/scene/scene.hpp"

//stl
#include <unordered_map>

namespace opengl
{
	struct Renderer::Impl
	{
		std::unordered_map<forces::Mesh*, std::vector<Mesh<VertexNormal>>> meshes_;
		std::vector<LightedObject> objects_;
		LightProgram lightProgram_;
		Camera camera_;

		void addNodeWithChildren(const forces::Node& node, glm::vec3 parentTranslation)
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
					auto &obj = objects_.emplace_back(mesh, lightProgram_);
					obj.postion() = thisTranslation;
				}
			}
			for(const auto &child : node.getChildren())
			{
				addNodeWithChildren(child, thisTranslation);
			}
		}
	};

	void Renderer::setCurrentScene(forces::Scene& scene)
	{
		pImpl_->addNodeWithChildren(scene.rootNode(), glm::vec3{ 0.f, 0.f, 0.f });
	}

	void Renderer::render()
	{
		GLCall(glEnable(GL_MULTISAMPLE));
		GLCall(glEnable(GL_CULL_FACE));
		GLCall(glEnable(GL_DEPTH_TEST));

		for (const auto& obj : pImpl_->objects_)
		{
			obj.draw(pImpl_->camera_.view_projection());
		}
	}
}

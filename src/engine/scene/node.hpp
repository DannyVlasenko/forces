#pragma once

//stl
#include <unordered_set>
#include <vec3.hpp>

//engine
#include "mesh.hpp"

namespace forces
{
	class INode
	{
	public:
		[[nodiscard]]
		const std::vector<INode>& getChildren() const noexcept
		{
			return children_;
		}
		[[nodiscard]]
		const std::unordered_set<Mesh*>& getMeshes() const noexcept
		{
			return meshes_;
		}
		[[nodiscard]]
		const glm::vec3& getTranslation() const noexcept
		{
			return translation_;
		}

	protected:
		std::vector<INode> children_;
		std::unordered_set<Mesh*> meshes_;
		glm::vec3 translation_{ 0.f, 0.f, 0.f };
		glm::vec3 scale_{ 1.f, 1.f, 1.f };

		INode() = default;
	};

	class Node : public INode
	{
	public:
		Node() = default;

		INode& addChild(INode &&child)
		{
			children_.push_back(std::move(child));
			return children_.back();
		}

		void addMesh(Mesh* mesh)
		{
			meshes_.insert(mesh);
		}
	};
}

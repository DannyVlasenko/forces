#pragma once

//stl
#include <unordered_set>
#include <vec3.hpp>

//engine
#include "mesh.hpp"

namespace forces
{
	class Node
	{
	public:
		[[nodiscard]]
		const std::vector<Node>& children() const noexcept
		{
			return children_;
		}

		[[nodiscard]]
		std::vector<Node>& children() noexcept
		{
			return children_;
		}

		[[nodiscard]]
		const std::unordered_set<Mesh*>& meshes() const noexcept
		{
			return meshes_;
		}

		[[nodiscard]]
		std::unordered_set<Mesh*>& meshes() noexcept
		{
			return meshes_;
		}

		[[nodiscard]]
		const glm::vec3& translation() const noexcept
		{
			return translation_;
		}

		[[nodiscard]]
		glm::vec3& translation() noexcept
		{
			return translation_;
		}

		Node& addChild(Node &&child)
		{
			children_.push_back(std::move(child));
			return children_.back();
		}

		void addMesh(Mesh* mesh)
		{
			meshes_.insert(mesh);
		}

	private:
		std::vector<Node> children_;
		std::unordered_set<Mesh*> meshes_;
		glm::vec3 translation_{ 0.f, 0.f, 0.f };
		glm::vec3 scale_{ 1.f, 1.f, 1.f };
	};
}

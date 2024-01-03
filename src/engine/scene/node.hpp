#pragma once
#include <unordered_set>
#include <vec3.hpp>

namespace forces
{
	class Node
	{
	public:
		void addChild(Node &&child)
		{
			children_.push_back(std::move(child));
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

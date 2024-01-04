#pragma once
#include <vector>

#include "mesh.hpp"
#include "node.hpp"

namespace forces
{
	class Scene
	{
	public:
		Node& rootNode() noexcept
		{
			return rootNode_;
		}

		const Node& rootNode() const noexcept
		{
			return rootNode_;
		}

		const std::vector<Mesh>& meshes() const
		{
			return meshes_;
		}
		
	private:
		Node rootNode_;
		std::vector<Mesh> meshes_;
	};
}

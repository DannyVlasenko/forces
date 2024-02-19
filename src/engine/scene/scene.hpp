#pragma once
#include <unordered_set>

#include "light.hpp"
#include "node.hpp"

namespace forces
{
	class Camera;

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
		
	private:
		Node rootNode_{L"Root"};
		std::unordered_set<Mesh> meshes_;
		Camera* selectedCamera_{nullptr};
		AmbientLight ambientLight_;
		std::vector<DirectedLight> directedLights_;
	};
}

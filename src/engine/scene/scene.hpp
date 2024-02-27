#pragma once
#include <span>
#include <unordered_set>

#include "camera_node.hpp"
#include "empty_node.hpp"
#include "light.hpp"
#include "material.hpp"
#include "mesh.hpp"
#include "node.hpp"

namespace forces
{
	class Scene
	{
	public:
		[[nodiscard]]
		EmptyNode& rootNode() noexcept
		{
			return rootNode_;
		}

		[[nodiscard]]
		const EmptyNode& rootNode() const noexcept
		{
			return rootNode_;
		}

		[[nodiscard]]
		AmbientLight& ambientLight() noexcept
		{
			return ambientLight_;
		}

		[[nodiscard]]
		const AmbientLight& ambientLight() const noexcept
		{
			return ambientLight_;
		}

		[[nodiscard]]
		std::vector<DirectedLight>& directedLights() noexcept
		{
			return directedLights_;
		}

		[[nodiscard]]
		std::span<const DirectedLight> directedLights() const noexcept
		{
			return directedLights_;
		}
		
	private:
		EmptyNode rootNode_{L"Root"};
		std::unordered_set<Mesh> meshes_;
		std::unordered_set<Material> materials_;
		AmbientLight ambientLight_;
		std::vector<DirectedLight> directedLights_;
		CameraNode* selectedCamera_{ nullptr };
	};
}

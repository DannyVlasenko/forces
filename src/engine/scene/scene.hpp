#pragma once
#include <span>
#include <unordered_set>

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
		Node& rootNode() noexcept
		{
			return rootNode_;
		}

		[[nodiscard]]
		const Node& rootNode() const noexcept
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

		[[nodiscard]]
		const Node * activeCameraNode() const noexcept
		{
			return activeCameraNode_;
		}

		void selectActiveCameraNode(Node* cameraNode) noexcept
		{
			activeCameraNode_ = cameraNode;
		}

	private:
		Node rootNode_{L"Root"};
		AmbientLight ambientLight_;
		std::vector<DirectedLight> directedLights_;
		Node* activeCameraNode_{ nullptr };
	};
}

#pragma once

namespace forces
{
	class Node;
	class Camera;

	class Renderer
	{
	public:
		virtual ~Renderer() = default;
		virtual void setCurrentRootNode(const Node& root) = 0;
		virtual void render() = 0;
		virtual void setCamera(Camera* camera) noexcept = 0;
	};
}

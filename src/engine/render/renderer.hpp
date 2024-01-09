#pragma once

namespace forces
{
	class Node;

	class Renderer
	{
	public:
		virtual ~Renderer() = default;
		virtual void setCurrentRootNode(const Node& root) = 0;
		virtual void render() = 0;
	};
}

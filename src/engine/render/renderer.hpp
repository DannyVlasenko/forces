#pragma once

namespace forces
{
	class INode;

	class Renderer
	{
	public:
		virtual ~Renderer() = default;
		virtual void setCurrentRootNode(const INode& root) = 0;
		virtual void render() = 0;
	};
}

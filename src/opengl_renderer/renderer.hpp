#pragma once

#include <memory>

#include "engine/render/renderer.hpp"

namespace opengl
{
	class Renderer : public forces::Renderer
	{
	public:
		Renderer();
		void setCurrentRootNode(const forces::INode& root) override;
		void render() override;

	private:
		struct Impl;
		std::unique_ptr<Impl> pImpl_;
	};
}

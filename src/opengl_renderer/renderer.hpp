#pragma once

#include <memory>

#include "engine/render/renderer.hpp"
#include "engine/scene/camera.hpp"

namespace opengl
{
	class Renderer : public forces::Renderer
	{
	public:
		Renderer();
		void setCurrentRootNode(const forces::Node& root) override;
		void render() override;
		void setCamera(forces::Camera* camera) noexcept override;

	private:
		struct Impl;
		std::unique_ptr<Impl> pImpl_;
	};
}

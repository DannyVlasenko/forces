#pragma once

#include <memory>

#include "engine/render/renderer.hpp"
#include "engine/render/window.hpp"

namespace opengl
{
	class Renderer : public forces::Renderer
	{
	public:
		explicit Renderer(forces::Window& window);
		void render() override;
		void processScene(forces::Scene& scene) override;

	private:
		struct Impl;
		std::unique_ptr<Impl> pImpl_;
	};
}

#pragma once

#include <memory>

#include "engine/render/renderer.hpp"

namespace opengl
{
	class Renderer : public forces::Renderer
	{
	public:
		void setCurrentScene(forces::Scene& scene) override;
		void render() override;

	private:
		struct Impl;
		std::unique_ptr<Impl> pImpl_;
	};
}

#pragma once

namespace forces
{
	class Scene;

	class Renderer
	{
	public:
		virtual ~Renderer() = default;
		virtual void setCurrentScene(Scene &scene) = 0;
		virtual void render() = 0;
	};
}

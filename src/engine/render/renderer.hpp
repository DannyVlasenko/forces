#pragma once

namespace forces
{
	class Node;
	class Camera;
	class Scene;

	class Renderer
	{
	public:
		virtual ~Renderer() = default;
		virtual void render() = 0;
		virtual void processScene(Scene& scene) = 0;
	};
}

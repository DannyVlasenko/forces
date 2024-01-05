#pragma once

namespace forces
{
	class Scene;

	class Engine
	{
	public:
		void setCurrentScene(Scene &scene);
		void step();
	};
}

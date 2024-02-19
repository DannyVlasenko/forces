#pragma once

namespace forces
{
	class Scene;

	class Engine
	{
	public:
		Scene& currentScene() noexcept;
		void tick();
		bool isRunning();
		explicit Engine(Scene& startup_scene);
	};
}

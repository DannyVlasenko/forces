#include "engine/engine.hpp"
#include "engine/scene/scene.hpp"
#include "glfwrap/window.hpp"
#include "opengl_renderer/renderer.hpp"

int main()
{
	//load startup scene
	forces::Scene startupScene;

	//create window
	glfw::Window mainWindow{ 800, 600, "MyProject" };

	//create engine
	forces::Engine engine{ startupScene };

	//create renderer
	opengl::Renderer renderer{ mainWindow };

	//run
	while (!mainWindow.should_close() && engine.isRunning()) {
		glfw::Window::process_events();
		engine.tick();
		renderer.processScene(engine.currentScene());
		renderer.render();
	}
}

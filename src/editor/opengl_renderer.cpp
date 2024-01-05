#include "opengl_renderer.h"

#include "engine/scene/node.hpp"
#include "opengl_renderer/renderer.hpp"

ForcesOpenGLRenderer* create_opengl_renderer()
{
	return reinterpret_cast<ForcesOpenGLRenderer*>(new opengl::Renderer);
}

void delete_opengl_renderer(ForcesOpenGLRenderer* renderer)
{
	delete reinterpret_cast<opengl::Renderer*>(renderer);
}

void opengl_renderer_set_root_node(ForcesOpenGLRenderer* renderer, ForcesNode* root)
{
	reinterpret_cast<opengl::Renderer*>(renderer)->setCurrentRootNode(*reinterpret_cast<forces::Node*>(root));
}

void opengl_renderer_render(ForcesOpenGLRenderer* renderer)
{
	reinterpret_cast<opengl::Renderer*>(renderer)->render();
}

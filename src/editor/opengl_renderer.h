#ifndef FORCES_OPENGL_RENDERER_H
#define FORCES_OPENGL_RENDERER_H

#include <GLFW/glfw3.h>

#include "camera.h"
#include "node.h"
#include "scene.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesOpenGLRenderer ForcesOpenGLRenderer;
typedef struct ForcesWindow ForcesWindow;

__declspec(dllexport)
ForcesWindow* adapt_glfw_window(GLFWwindow* glfwWindow);

__declspec(dllexport)
void delete_window_adapter(ForcesWindow* window);

__declspec(dllexport)
ForcesOpenGLRenderer * create_opengl_renderer(ForcesWindow* window);

__declspec(dllexport)
void delete_opengl_renderer(ForcesOpenGLRenderer *renderer);

__declspec(dllexport)
void opengl_renderer_process_scene(ForcesOpenGLRenderer *renderer, ForcesScene *scene);

__declspec(dllexport)
void opengl_renderer_render(ForcesOpenGLRenderer *renderer);

#ifdef __cplusplus
}
#endif
#endif // FORCES_OPENGL_RENDERER_H

#ifndef FORCES_OPENGL_RENDERER_H
#define FORCES_OPENGL_RENDERER_H

#include "node.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesOpenGLRenderer ForcesOpenGLRenderer;

__declspec(dllexport)
ForcesOpenGLRenderer * create_opengl_renderer();

__declspec(dllexport)
void delete_opengl_renderer(ForcesOpenGLRenderer *renderer);

__declspec(dllexport)
void opengl_renderer_set_root_node(ForcesOpenGLRenderer *renderer, ForcesNode *root);

__declspec(dllexport)
void opengl_renderer_render(ForcesOpenGLRenderer *renderer);

#ifdef __cplusplus
}
#endif
#endif // FORCES_OPENGL_RENDERER_H

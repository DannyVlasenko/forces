#ifndef FORCES_SCENE_H
#define FORCES_SCENE_H

#include "node.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesScene ForcesScene;

__declspec(dllexport)
ForcesScene * create_scene();

__declspec(dllexport)
void delete_scene(ForcesScene *scene);

__declspec(dllexport)
ForcesNode * scene_root_node(ForcesScene *scene);

#ifdef __cplusplus
}
#endif
#endif // FORCES_SCENE_H

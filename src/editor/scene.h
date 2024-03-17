#ifndef FORCES_SCENE_H
#define FORCES_SCENE_H

#include "light.h"
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

__declspec(dllexport)
ForcesAmbientLight * scene_ambient_light(ForcesScene *scene);

__declspec(dllexport)
ForcesDirectedLight * scene_create_directed_light(ForcesScene* scene, const wchar_t* name);

__declspec(dllexport)
void scene_remove_directed_light(ForcesScene* scene, ForcesDirectedLight* directedLight);

__declspec(dllexport)
void scene_set_active_camera_node(ForcesScene* scene, ForcesNode* cameraNode);

#ifdef __cplusplus
}
#endif
#endif // FORCES_SCENE_H

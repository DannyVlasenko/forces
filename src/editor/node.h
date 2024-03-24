#ifndef FORCES_NODE_H
#define FORCES_NODE_H

#include "camera.h"
#include "glm_wrap.h"
#include "light.h"
#include "material.h"
#include "mesh.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesNode ForcesNode;

__declspec(dllexport)
ForcesNode * create_empty_node(ForcesNode *parent, const wchar_t *name);

__declspec(dllexport)
ForcesNode * create_camera_node(ForcesNode *parent, const wchar_t *name);

__declspec(dllexport)
ForcesNode * create_light_node(ForcesNode *parent, const wchar_t *name);

__declspec(dllexport)
ForcesNode * create_mesh_node(ForcesNode *parent, const wchar_t *name, ForcesMesh *mesh, ForcesMaterial *material);

__declspec(dllexport)
void node_set_name(ForcesNode* node, const wchar_t* name);

__declspec(dllexport)
void node_set_translation(ForcesNode *node, vec3 translation);

__declspec(dllexport)
void node_set_scale(ForcesNode *node, vec3 scale);

__declspec(dllexport)
void node_set_rotation(ForcesNode *node, vec4 rotation);

__declspec(dllexport)
void node_set_mesh(ForcesNode* node, ForcesMesh* mesh);

__declspec(dllexport)
void node_set_material(ForcesNode* node, ForcesMaterial* material);

__declspec(dllexport)
ForcesPointLight* node_get_light(ForcesNode* node);

__declspec(dllexport)
ForcesCamera* node_get_camera(ForcesNode* node);

#ifdef __cplusplus
}
#endif
#endif // FORCES_NODE_H

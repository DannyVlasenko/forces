#ifndef FORCES_NODE_H
#define FORCES_NODE_H

#include "glm_wrap.h"
#include "mesh.h"
#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesNode ForcesNode;

__declspec(dllexport)
ForcesNode * create_node(ForcesNode *parent);

__declspec(dllexport)
void node_add_mesh(ForcesNode *node, ForcesMesh *mesh);

__declspec(dllexport)
int node_children_count(ForcesNode *node);

__declspec(dllexport)
int node_get_children(ForcesNode *node, ForcesNode* outChildren[], int outChildrenCount);

__declspec(dllexport)
vec3 node_get_translation(ForcesNode *node);

__declspec(dllexport)
void node_set_translation(ForcesNode *node, vec3 translation);

#ifdef __cplusplus
}
#endif
#endif // FORCES_NODE_H

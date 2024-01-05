#ifndef FORCES_NODE_H
#define FORCES_NODE_H
#include "mesh.h"
#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesNode ForcesNode;

ForcesNode * create_node(ForcesNode *parent);

void node_add_mesh(ForcesNode *node, ForcesMesh *mesh);

#ifdef __cplusplus
}
#endif
#endif // FORCES_NODE_H

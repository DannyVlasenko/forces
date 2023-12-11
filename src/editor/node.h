#ifndef FORCES_NODE_H
#define FORCES_NODE_H
#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesNode ForcesNode;

ForcesNode * create_node(ForcesNode *parent, const char *name);

void delete_node(ForcesNode *node);

#ifdef __cplusplus
}
#endif
#endif // FORCES_NODE_H

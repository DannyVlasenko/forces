#include "node.h"
#include "engine/scene/node.hpp"

ForcesNode* create_node(ForcesNode *parent)
{	
	return reinterpret_cast<ForcesNode*>(&reinterpret_cast<forces::Node*>(parent)->addChild(forces::Node{}));
}

void node_add_mesh(ForcesNode* node, ForcesMesh *mesh)
{
	reinterpret_cast<forces::Node*>(node)->addMesh(reinterpret_cast<forces::Mesh*>(mesh));
}

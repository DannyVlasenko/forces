#include "node.h"
#include "engine/scene/node.hpp"

ForcesNode* create_node(ForcesNode *parent, const wchar_t* name)
{	
	return reinterpret_cast<ForcesNode*>(&reinterpret_cast<forces::Node*>(parent)->addChild(forces::Node{ name }));
}

void node_set_mesh(ForcesNode* node, ForcesMesh *mesh)
{
	reinterpret_cast<forces::Node*>(node)->setMesh(reinterpret_cast<forces::Mesh*>(mesh));
}

int node_children_count(ForcesNode* node)
{
	return reinterpret_cast<forces::Node*>(node)->children().size();
}

int node_get_children(ForcesNode* node, ForcesNode* outChildren[], int outChildrenCount)
{
	auto &children = reinterpret_cast<forces::Node*>(node)->children();
	auto i = 0;
	for (; i < outChildrenCount && i < children.size(); ++i)
	{
		outChildren[i] = reinterpret_cast<ForcesNode*>(&children[i]);
	}
	return i;
}

vec3 node_get_translation(ForcesNode* node)
{
	const auto& translation = reinterpret_cast<forces::Node*>(node)->translation();
	return vec3{ translation.x, translation.y, translation.z };
}

void node_set_translation(ForcesNode* node, vec3 translation)
{
	auto& nodeTranslation = reinterpret_cast<forces::Node*>(node)->translation();
	nodeTranslation.x = translation.x;
	nodeTranslation.y = translation.y;
	nodeTranslation.z = translation.z;
}

const wchar_t* node_get_name(ForcesNode* node)
{
	return reinterpret_cast<forces::Node*>(node)->name().c_str();
}

void node_set_name(ForcesNode* node, const wchar_t* name)
{
	reinterpret_cast<forces::Node*>(node)->name() = name;
}

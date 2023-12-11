#include "scene.h"

#include <assimp/scene.h>

ForcesScene* create_scene()
{
	auto * scene = new aiScene;
	scene->mRootNode = new aiNode;
	return reinterpret_cast<ForcesScene*>(scene);
}

void delete_scene(ForcesScene* scene)
{
	auto * sc = reinterpret_cast<aiScene*>(scene);
	delete sc->mRootNode;
	delete sc;
}

ForcesNode* root_node(ForcesScene* scene)
{
	auto * sc = reinterpret_cast<aiScene*>(scene);
	return reinterpret_cast<ForcesNode*>(sc->mRootNode);
}

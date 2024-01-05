//editor
#include "scene.h"

//engine
#include "engine/scene/scene.hpp"


ForcesScene* create_scene()
{
	auto * scene = new forces::Scene;
	return reinterpret_cast<ForcesScene*>(scene);
}

void delete_scene(ForcesScene* scene)
{
	const auto * sc = reinterpret_cast<forces::Scene*>(scene);
	delete sc;
}

ForcesNode* scene_root_node(ForcesScene* scene)
{
	auto * sc = reinterpret_cast<forces::Scene*>(scene);
	return reinterpret_cast<ForcesNode*>(&sc->rootNode());
}

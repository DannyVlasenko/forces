//editor
#include "scene.h"

//engine
#include "engine/scene/scene.hpp"


ForcesScene* create_scene()
{
	return reinterpret_cast<ForcesScene*>(new forces::Scene);
}

void delete_scene(ForcesScene* scene)
{
	delete reinterpret_cast<forces::Scene*>(scene);
}

ForcesNode* scene_root_node(ForcesScene* scene)
{
	auto * sc = reinterpret_cast<forces::Scene*>(scene);
	return reinterpret_cast<ForcesNode*>(&sc->rootNode());
}

ForcesAmbientLight* scene_ambient_light(ForcesScene* scene)
{
	auto* sc = reinterpret_cast<forces::Scene*>(scene);
	return reinterpret_cast<ForcesAmbientLight*>(&sc->ambientLight());
}

ForcesDirectedLight* scene_create_directed_light(ForcesScene* scene, const wchar_t* name)
{
	auto* sc = reinterpret_cast<forces::Scene*>(scene);
	return reinterpret_cast<ForcesDirectedLight*>(&sc->directedLights().emplace_back());
}

void scene_set_active_camera_node(ForcesScene* scene, ForcesNode* cameraNode)
{
	auto* sc = reinterpret_cast<forces::Scene*>(scene);
	sc->selectActiveCameraNode(reinterpret_cast<forces::CameraNode*>(cameraNode));
}

#include "node.h"
#include "engine/scene/node.hpp"

ForcesNode* create_empty_node(ForcesNode *parent, const wchar_t* name)
{
	auto& newEmptyNode = reinterpret_cast<forces::Node*>(parent)->addChild(forces::Node{ name });
	return reinterpret_cast<ForcesNode*>(&newEmptyNode);
}

ForcesNode* create_camera_node(ForcesNode* parent, const wchar_t* name)
{
	auto& newCameraNode = reinterpret_cast<forces::Node*>(parent)->addChild(forces::Node{ name, forces::Camera{} });
	return reinterpret_cast<ForcesNode*>(&newCameraNode);
}

ForcesNode* create_light_node(ForcesNode* parent, const wchar_t* name)
{
	auto& newLightNode = reinterpret_cast<forces::Node*>(parent)->addChild(forces::Node{ name, forces::PointLight{} });
	return reinterpret_cast<ForcesNode*>(&newLightNode);
}

ForcesNode* create_mesh_node(ForcesNode* parent, const wchar_t* name, ForcesMesh* mesh, ForcesMaterial* material)
{
	const forces::MeshContent meshContent
	{
		reinterpret_cast<forces::Mesh*>(mesh),
		reinterpret_cast<forces::Material*>(material)
	};
	auto& newMeshNode = reinterpret_cast<forces::Node*>(parent)->addChild(forces::Node{ name, meshContent });
	return reinterpret_cast<ForcesNode*>(&newMeshNode);
}

void node_set_name(ForcesNode* node, const wchar_t* name)
{
	reinterpret_cast<forces::Node*>(node)->name() = name;
}

void node_set_mesh(ForcesNode* node, ForcesMesh *mesh)
{
	auto& meshContent = std::get<forces::MeshContent>(reinterpret_cast<forces::Node*>(node)->content());
	meshContent.setMesh(reinterpret_cast<forces::Mesh*>(mesh));
}

void node_set_material(ForcesNode* node, ForcesMaterial *material)
{
	auto& meshContent = std::get<forces::MeshContent>(reinterpret_cast<forces::Node*>(node)->content());
	meshContent.setMaterial(reinterpret_cast<forces::Material*>(material));
}

void node_set_translation(ForcesNode* node, vec3 translation)
{
	auto& nodeTranslation = reinterpret_cast<forces::Node*>(node)->translation();
	nodeTranslation.x = translation.x;
	nodeTranslation.y = translation.y;
	nodeTranslation.z = translation.z;
}

void node_set_scale(ForcesNode* node, vec3 scale)
{
	auto& nodeScale = reinterpret_cast<forces::Node*>(node)->scale();
	nodeScale.x = scale.x;
	nodeScale.y = scale.y;
	nodeScale.z = scale.z;
}

void node_set_rotation(ForcesNode* node, vec4 rotation)
{
	auto& nodeRotation = reinterpret_cast<forces::Node*>(node)->rotation();
	nodeRotation.x = rotation.x;
	nodeRotation.y = rotation.y;
	nodeRotation.z = rotation.z;
	nodeRotation.w = rotation.w;
}

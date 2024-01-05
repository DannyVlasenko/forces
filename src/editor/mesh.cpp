#include "mesh.h"
#include "engine/scene/mesh.hpp"

ForcesMesh* create_mesh(const wchar_t* path)
{
	return reinterpret_cast<ForcesMesh*>(new forces::Mesh(path));
}

void delete_mesh(ForcesMesh* mesh)
{
	delete reinterpret_cast<forces::Mesh*>(mesh);
}

const wchar_t* mesh_get_path(ForcesMesh* mesh)
{
	return reinterpret_cast<forces::Mesh*>(mesh)->path().c_str();
}

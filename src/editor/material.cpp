#include "material.h"
#include "engine/scene/material.hpp"

ForcesMaterial* create_material()
{
	return reinterpret_cast<ForcesMaterial*>(new forces::Material);
}

void delete_material(ForcesMaterial* material)
{
	delete reinterpret_cast<forces::Material*>(material);
}

void material_set_color(ForcesMaterial* material, vec3 color)
{
	reinterpret_cast<forces::Material*>(material)->color().x = color.x;
	reinterpret_cast<forces::Material*>(material)->color().y = color.y;
	reinterpret_cast<forces::Material*>(material)->color().z = color.z;
}

#include "light.h"
#include "engine/scene/light.hpp"

void light_set_color(ForcesLight* light, vec3 color)
{
	reinterpret_cast<forces::Light*>(light)->color().x = color.x;
	reinterpret_cast<forces::Light*>(light)->color().y = color.y;
	reinterpret_cast<forces::Light*>(light)->color().z = color.z;
}

void directed_light_set_direction(ForcesDirectedLight* light, vec3 direction)
{
	reinterpret_cast<forces::DirectedLight*>(light)->direction().x = direction.x;
	reinterpret_cast<forces::DirectedLight*>(light)->direction().y = direction.y;
	reinterpret_cast<forces::DirectedLight*>(light)->direction().z = direction.z;
}

void point_light_set_strength(ForcesPointLight* light, float strength)
{
	reinterpret_cast<forces::PointLight*>(light)->strength = strength;
}

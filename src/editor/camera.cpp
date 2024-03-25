#include "camera.h"
#include "engine/scene/camera.hpp"
#include "engine/scene/node.hpp"

ForcesCamera* create_camera()
{
	return reinterpret_cast<ForcesCamera*>(new forces::Camera);
}

void delete_camera(ForcesCamera* camera)
{
	delete reinterpret_cast<forces::Camera *>(camera);
}

float camera_get_fov(ForcesCamera* camera)
{
	return reinterpret_cast<forces::Camera *>(camera)->fov();
}

void camera_set_fov(ForcesCamera* camera, float fov)
{
	reinterpret_cast<forces::Camera *>(camera)->fov() = fov;
}

float camera_get_far(ForcesCamera* camera)
{
	return reinterpret_cast<forces::Camera *>(camera)->far();
}

void camera_set_far(ForcesCamera* camera, float far)
{
	reinterpret_cast<forces::Camera *>(camera)->far() = far;
}

float camera_get_near(ForcesCamera* camera)
{
	return reinterpret_cast<forces::Camera *>(camera)->near();
}

void camera_set_near(ForcesCamera* camera, float near)
{
	reinterpret_cast<forces::Camera *>(camera)->near() = near;
}

vec2 camera_get_viewport(ForcesCamera* camera)
{
	const auto& viewport = reinterpret_cast<forces::Camera *>(camera)->viewport();
	return { viewport.x, viewport.y };
}

void camera_set_viewport(ForcesCamera* camera, vec2 vp)
{
	auto& viewport = reinterpret_cast<forces::Camera *>(camera)->viewport();
	viewport.x = vp.x;
	viewport.y = vp.y;
}

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

vec3 camera_get_position(ForcesCamera* camera)
{
	const auto& position = reinterpret_cast<forces::Camera *>(camera)->position();
	return { position.x, position.y, position.z };
}

void camera_set_position(ForcesCamera* camera, vec3 pos)
{
	auto& position = reinterpret_cast<forces::Camera *>(camera)->position();
	position.x = pos.x;
	position.y = pos.y;
	position.z = pos.z;
}

void camera_yaw(ForcesCamera* camera, float grad)
{
	yaw(*reinterpret_cast<forces::Camera *>(camera), grad);
}

void camera_pitch(ForcesCamera* camera, float grad)
{
	pitch(*reinterpret_cast<forces::Camera *>(camera), grad);
}

void camera_roll(ForcesCamera* camera, float grad)
{
	roll(*reinterpret_cast<forces::Camera *>(camera), grad);
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

vec4 camera_get_rotation(ForcesCamera* camera)
{
	const auto& rotation = reinterpret_cast<forces::Camera*>(camera)->rotation();
	return { rotation.w, rotation.x, rotation.y, rotation.z };
}

void camera_set_rotation(ForcesCamera* camera, vec4 rot)
{
	auto& rotation = reinterpret_cast<forces::Camera*>(camera)->rotation();
	rotation.w = rot.w;
	rotation.x = rot.x;
	rotation.y = rot.y;
	rotation.z = rot.z;
}

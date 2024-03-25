#ifndef FORCES_CAMERA_H
#define FORCES_CAMERA_H
#include "glm_wrap.h"
#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesCamera ForcesCamera;

__declspec(dllexport)
ForcesCamera * create_camera();

__declspec(dllexport)
void delete_camera(ForcesCamera *camera);

__declspec(dllexport)
float camera_get_fov(ForcesCamera *camera);

__declspec(dllexport)
void camera_set_fov(ForcesCamera *camera, float fov);

__declspec(dllexport)
float camera_get_far(ForcesCamera *camera);

__declspec(dllexport)
void camera_set_far(ForcesCamera *camera, float far);

__declspec(dllexport)
float camera_get_near(ForcesCamera *camera);

__declspec(dllexport)
void camera_set_near(ForcesCamera *camera, float near);

__declspec(dllexport)
vec2 camera_get_viewport(ForcesCamera *camera);

__declspec(dllexport)
void camera_set_viewport(ForcesCamera *camera, vec2 vp);

#ifdef __cplusplus
}
#endif
#endif // FORCES_CAMERA_H

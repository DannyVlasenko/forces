#ifndef FORCES_LIGHT_H
#define FORCES_LIGHT_H

#include "glm_wrap.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesLight ForcesLight;
typedef struct ForcesAmbientLight ForcesAmbientLight;
typedef struct ForcesDirectedLight ForcesDirectedLight;
typedef struct ForcesPointLight ForcesPointLight;

__declspec(dllexport)
void light_set_color(ForcesLight* light, vec3 color);

__declspec(dllexport)
void directed_light_set_direction(ForcesDirectedLight* light, vec3 direction);

__declspec(dllexport)
void point_light_set_strength(ForcesPointLight* light, float strength);

#ifdef __cplusplus
}
#endif

#endif // FORCES_LIGHT_H

#ifndef FORCES_MATERIAL_H
#define FORCES_MATERIAL_H

#include "glm_wrap.h"

#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesMaterial ForcesMaterial;

__declspec(dllexport)
ForcesMaterial* create_material();

__declspec(dllexport)
void delete_material(ForcesMaterial* material);

__declspec(dllexport)
void material_set_color(ForcesMaterial* material, vec3 color);

#ifdef __cplusplus
}
#endif

#endif // FORCES_MATERIAL_H

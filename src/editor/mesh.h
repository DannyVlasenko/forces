#ifndef FORCES_MESH_H
#define FORCES_MESH_H

#ifdef __cplusplus
extern "C" {
#endif

typedef struct ForcesMesh ForcesMesh;

ForcesMesh * create_mesh(const wchar_t *path);

void delete_mesh(ForcesMesh *mesh);

const wchar_t* mesh_get_path(ForcesMesh *mesh);

#ifdef __cplusplus
}
#endif

#endif // FORCES_MESH_H

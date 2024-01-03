#include "scene_object.hpp"

#include <ext/matrix_transform.hpp>

namespace models
{
    SceneObject::SceneObject(const opengl::Mesh<opengl::VertexNormal> &mesh, const opengl::Program& program):
        mMesh(mesh),
        mProgram(program)
    {}

    void SceneObject::draw(const glm::mat4& view_projection, GLenum mode) const
    {
        mProgram.bind();
        auto model = glm::mat4(1.f);
        model = translate(model, mPosition);
        model = glm::scale(model, mScale);
        mProgram.set_uniform("model", model);
        mProgram.set_uniform("viewProjection", view_projection);
        mMesh.draw(mode);
        mProgram.unbind();
    }
}

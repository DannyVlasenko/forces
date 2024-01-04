#include "lighted_object.hpp"

namespace opengl
{
    LightedObject::LightedObject(const opengl::Mesh<opengl::VertexNormal>& mesh, const LightProgram& light_program):
        SceneObject(mesh, light_program),
        mProgram(light_program)
    {}

    void LightedObject::draw(const glm::mat4& view_projection, GLenum mode) const
    {
        mProgram.set_uniform("objectColor", mColor);
        SceneObject::draw(view_projection, mode);
    }
}

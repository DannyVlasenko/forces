#ifndef MODELS_LIGHTED_OBJECT_HPP
#define MODELS_LIGHTED_OBJECT_HPP

#include "light_program.hpp"
#include "scene_object.hpp"

namespace opengl
{
    class LightedObject : public SceneObject
    {
    public:
        LightedObject(const opengl::Mesh<opengl::VertexNormal>& mesh, const LightProgram &light_program);

        glm::vec3& color() noexcept
        {
            return mColor;
        }

        void draw(const glm::mat4& view_projection, GLenum mode = GL_TRIANGLES) const override;

    private:
        const LightProgram& mProgram;
        glm::vec3 mColor{ 1.0f };
    };
}
#endif // MODELS_LIGHTED_OBJECT_HPP

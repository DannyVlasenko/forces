#ifndef MODELS_LIGHT_PROGRAM_HPP
#define MODELS_LIGHT_PROGRAM_HPP

#include "opengl/shader.hpp"

namespace opengl
{
    class LightProgram : public opengl::Program
    {
    public:
        LightProgram();

        void setAmbientLightColor(const glm::vec3& ambient_color) const;

        void setPointLightColor(const glm::vec3& point_color) const;

        void setPointLightPosition(const glm::vec3& point_position) const;

        void setDirectedLightColor(const glm::vec3& directed_color) const;

        void setDirectedLightOrientation(const glm::vec3& direction) const;
    };
}
#endif // MODELS_LIGHT_PROGRAM_HPP

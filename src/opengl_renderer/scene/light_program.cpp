#include "light_program.hpp"

namespace models
{
    namespace
    {
        opengl::ShaderSource VertexSrc
        {
            .type = GL_VERTEX_SHADER,
            .code =
            R"--(
#version 330

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 inNormal;

uniform mat4 viewProjection;
uniform mat4 model;

out vec3 normal;
out vec3 fragPos;

void main()
{	
	fragPos = vec3(model * vec4(position, 1.0));
	gl_Position = viewProjection * vec4(fragPos, 1.0);
	normal = transpose(inverse(mat3(model))) * inNormal;
}
)--"
        };

        opengl::ShaderSource FragmentSrc
        {
            .type = GL_FRAGMENT_SHADER,
            .code =
            R"--(
#version 330

in vec3 normal;
in vec3 fragPos;

layout(location = 0) out vec4 color;

uniform vec3 objectColor;
uniform vec3 ambientLightColor;
uniform vec3 pointLightPosition;  
uniform vec3 pointLightColor;
uniform vec3 directedLightOrient;  
uniform vec3 directedLightColor;    

void main()
{
	vec3 norm = normalize(normal);

	vec3 pointLightDir = normalize(pointLightPosition - fragPos);
	float pointLightDistance = distance(pointLightPosition, fragPos);
	float pointDiff = max(dot(norm, pointLightDir), 0.0);
	vec3 pointDiffuseColor = pointDiff * pointLightColor / (0.05 * pow(pointLightDistance, 2));

	float directedDiff = max(dot(norm, normalize(directedLightOrient)), 0.0);
	vec3 directedDiffuseColor = directedDiff * directedLightColor;

	vec3 resColor = (directedDiffuseColor + pointDiffuseColor + ambientLightColor) * objectColor;
	color = vec4(resColor, 1.0);
}
)--"
        };
    }

    LightProgram::LightProgram() :
        Program(opengl::Shader{ VertexSrc }, opengl::Shader{ FragmentSrc })
    {}

    void LightProgram::setAmbientLightColor(const glm::vec3& ambient_color) const
    {
        set_uniform("ambientLightColor", ambient_color);
    }

    void LightProgram::setPointLightColor(const glm::vec3& point_color) const
    {
        set_uniform("pointLightColor", point_color);
    }

    void LightProgram::setPointLightPosition(const glm::vec3& point_position) const
    {
        set_uniform("pointLightPosition", point_position);
    }

    void LightProgram::setDirectedLightColor(const glm::vec3& directed_color) const
    {
        set_uniform("directedLightColor", directed_color);
    }

    void LightProgram::setDirectedLightOrientation(const glm::vec3& direction) const
    {
        set_uniform("directedLightOrient", direction);
    }

}

#ifndef MODELS_SCENE_OBJECT_HPP
#define MODELS_SCENE_OBJECT_HPP

#include "opengl/mesh.hpp"
#include "opengl/shader.hpp"

namespace opengl
{
	class IDrawable
	{
	public:
		virtual ~IDrawable() = default;
		virtual void draw(const glm::mat4& view_projection, GLenum mode = GL_TRIANGLES) const = 0;
	};

	class SceneObject : public IDrawable
	{
	public:
		SceneObject(const opengl::Mesh<opengl::VertexNormal>& mesh, const opengl::Program& program);

		void draw(const glm::mat4& view_projection, GLenum mode = GL_TRIANGLES) const override;

        glm::vec3& postion() noexcept
		{
			return mPosition;
		}

		glm::vec3& scale() noexcept
		{
			return mScale;
		}

	private:
		const opengl::Mesh<opengl::VertexNormal> &mMesh;
		const opengl::Program& mProgram;
		glm::vec3 mPosition{ 0.0f };
		glm::vec3 mScale{ 1.0f };

	};
}
#endif // MODELS_SCENE_OBJECT_HPP

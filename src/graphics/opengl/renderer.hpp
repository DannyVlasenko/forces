#ifndef OPENGL_RENDERER_HPP
#define OPENGL_RENDERER_HPP
#include "shader.hpp"
#include "vertex_array.hpp"

namespace opengl
{
	class Renderer
	{
	public:
		void draw(const VertexArray& va, const IndexBuffer& ib, const Program& program)
		{
			program.bind();
			va.bind();
			ib.bind();
			GLCall(glDrawElements(GL_TRIANGLES, ib.count(), GL_UNSIGNED_INT, nullptr));
		}
	};
}
#endif // OPENGL_RENDERER_HPP

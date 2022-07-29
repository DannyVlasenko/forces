#ifndef OPENGL_VERTEX_ARRAY_HPP
#define OPENGL_VERTEX_ARRAY_HPP
#include <vector>

#include "buffer.hpp"

namespace opengl
{
	template <typename T>
	constexpr GLenum gl_type()
	{
		static_assert(false);
	}

	template <>
	constexpr GLenum gl_type<float>()
	{
		return GL_FLOAT;
	}

	class VertexBufferLayout
	{
	public:

		template <typename T>
		VertexBufferLayout & push(GLuint count, GLboolean normalized = GL_FALSE)
		{
			mElements.emplace_back(count, gl_type<T>(), normalized);
			return *this;
		}

	private:
		struct Element
		{
			GLuint count;
			GLenum type;
			GLboolean normalized;
		};

		std::vector<Element> mElements;
	};

	class VertexArray
	{
	public:
		VertexArray& add_buffer(const VertexBuffer& vb, const VertexBufferLayout& layout)
		{
			
		}
	};
}

#endif // OPENGL_VERTEX_ARRAY_HPP

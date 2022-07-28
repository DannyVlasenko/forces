#ifndef VERTEX_BUFFER_HPP
#define VERTEX_BUFFER_HPP

#include <memory>
#include "glcall.hpp"

namespace opengl
{
	class VertexBuffer
	{
	public:
		VertexBuffer(const void* data, GLuint size);

		VertexBuffer(const VertexBuffer& other) = delete;
		VertexBuffer& operator=(const VertexBuffer& other) = delete;

		VertexBuffer(VertexBuffer&& other) noexcept;
		VertexBuffer& operator=(VertexBuffer&& other) noexcept;

		~VertexBuffer();

		void bind() const;

		void unbind() const;

	private:
		GLuint mBuffer;
	};
}

#endif // VERTEX_BUFFER_HPP

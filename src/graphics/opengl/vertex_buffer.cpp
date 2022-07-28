#include "vertex_buffer.hpp"

namespace opengl
{
	VertexBuffer::VertexBuffer(const void* data, GLuint size)
	{
		GLCall(glGenBuffers(1, &mBuffer));
		GLCall(glBindBuffer(GL_ARRAY_BUFFER, mBuffer));
		GLCall(glBufferData(GL_ARRAY_BUFFER, size, data, GL_STATIC_DRAW));
	}

	VertexBuffer::VertexBuffer(VertexBuffer&& other) noexcept : mBuffer(other.mBuffer)
	{
		other.mBuffer = 0;
	}

	VertexBuffer& VertexBuffer::operator=(VertexBuffer&& other) noexcept
	{
		std::swap(mBuffer, other.mBuffer);
		return *this;
	}

	VertexBuffer::~VertexBuffer()
	{
		GLCall(glDeleteBuffers(1, &mBuffer));
	}

	void VertexBuffer::bind() const
	{
		GLCall(glBindBuffer(GL_ARRAY_BUFFER, mBuffer));
	}

	void VertexBuffer::unbind() const
	{
		GLCall(glBindBuffer(GL_ARRAY_BUFFER, 0));
	}
}

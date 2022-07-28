#include "index_buffer.hpp"

namespace opengl
{
	IndexBuffer::IndexBuffer(const GLuint* data, GLuint count):
		mCount(count)
	{
		GLCall(glGenBuffers(1, &mBuffer));
		GLCall(glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mBuffer));
		GLCall(glBufferData(GL_ELEMENT_ARRAY_BUFFER, count*sizeof(GLuint), data, GL_STATIC_DRAW));
	}

	IndexBuffer::IndexBuffer(IndexBuffer&& other) noexcept :
		mBuffer(other.mBuffer),
		mCount(other.mCount)
	{
		other.mBuffer = 0;
		other.mCount = 0;
	}

	IndexBuffer& IndexBuffer::operator=(IndexBuffer&& other) noexcept
	{
		std::swap(mBuffer, other.mBuffer);
		std::swap(mCount, other.mCount);
		return *this;
	}

	IndexBuffer::~IndexBuffer()
	{
		GLCall(glDeleteBuffers(1, &mBuffer));
	}

	void IndexBuffer::bind() const
	{
		GLCall(glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mBuffer));
	}

	void IndexBuffer::unbind() const
	{
		GLCall(glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0));
	}

	GLuint IndexBuffer::count() const noexcept
	{
		return mCount;
	}
}

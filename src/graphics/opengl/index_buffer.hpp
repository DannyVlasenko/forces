#ifndef INDEX_BUFFER_HPP
#define INDEX_BUFFER_HPP

#include <span>

#include "glcall.hpp"

namespace opengl
{
	class IndexBuffer
	{
	public:
		explicit IndexBuffer(std::span<const GLuint> indices);

		IndexBuffer(const IndexBuffer& other) = delete;
		IndexBuffer& operator=(const IndexBuffer& other) = delete;

		IndexBuffer(IndexBuffer&& other) noexcept;
		IndexBuffer& operator=(IndexBuffer&& other) noexcept;

		~IndexBuffer();

		void bind() const;

		void unbind() const;

		GLuint count() const noexcept;

	private:
		GLuint mBuffer;
		GLuint mCount;
	};
}

#endif // INDEX_BUFFER_HPP

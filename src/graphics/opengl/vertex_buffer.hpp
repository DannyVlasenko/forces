#ifndef VERTEX_BUFFER_HPP
#define VERTEX_BUFFER_HPP

#include "buffer.hpp"

namespace opengl
{
	template <typename T>
	class VertexBffer : public Buffer<GL_ARRAY_BUFFER, T>
	{
	public:
		
		VertexBuffer(std::span<const T> data):
			Buffer(std::move(data))
		{}
	};

	
}

#endif // VERTEX_BUFFER_HPP

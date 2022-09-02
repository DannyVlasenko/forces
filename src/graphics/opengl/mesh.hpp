#ifndef OPENGL_MESH_HPP
#define OPENGL_MESH_HPP

#include <filesystem>
#include <vec3.hpp>
#include <vector>

#include "buffer.hpp"
#include "vertex_array.hpp"

namespace opengl
{
	struct Vertex
	{
		glm::vec3 position;
		glm::vec3 normal;

		static VertexBufferLayout layout();
	};

	class Mesh
	{
	public:
		Mesh(std::span<Vertex> vertices, std::span<GLuint> indices);

		void draw() const;

	private:
		VertexArray mVertexArray;
		VertexBuffer<Vertex> mVertexBuffer;
		IndexBuffer mIndexBuffer;
	};

	Mesh cube_mesh();

	std::vector<Mesh> load_from_file(const std::filesystem::path& file);
}
#endif // OPENGL_MESH_HPP

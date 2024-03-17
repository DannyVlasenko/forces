#pragma once

namespace forces
{
	class Mesh;
	class Material;

	class MeshContent
	{
	public:
		MeshContent(const Mesh *mesh, const Material* material):
			mesh_(mesh),
			material_(material)
		{}

		[[nodiscard]]
		const Mesh * mesh() const noexcept
		{
			return mesh_;
		}

		void setMesh(const Mesh* mesh) noexcept
		{
			mesh_ = mesh;
		}

		[[nodiscard]]
		const Material* material() const noexcept
		{
			return material_;
		}

		void setMaterial(const Material* material) noexcept
		{
			material_ = material;
		}

	private:
		const Mesh* mesh_;
		const Material* material_;
	};
}
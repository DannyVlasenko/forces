#pragma once

//stl
#include <unordered_set>
#include <vec3.hpp>

//engine
#include "mesh.hpp"

namespace forces
{
	class Node
	{
	public:
		explicit Node(const wchar_t *name):
			name_(name)
		{}

		[[nodiscard]]
		const std::vector<Node>& children() const noexcept
		{
			return children_;
		}

		[[nodiscard]]
		std::vector<Node>& children() noexcept
		{
			return children_;
		}

		[[nodiscard]]
		const glm::vec3& translation() const noexcept
		{
			return translation_;
		}

		[[nodiscard]]
		glm::vec3& translation() noexcept
		{
			return translation_;
		}

		Node& addChild(Node &&child)
		{
			children_.push_back(std::move(child));
			return children_.back();
		}

		void setMesh(Mesh* mesh) noexcept
		{
			mesh_ = mesh;
		}

		[[nodiscard]]
		Mesh * getMesh() const noexcept
		{
			return mesh_;
		}

		[[nodiscard]]
		std::wstring& name() noexcept
		{
			return name_;
		}

		[[nodiscard]]
		const std::wstring& name() const noexcept
		{
			return name_;
		}

	private:
		std::wstring name_;
		std::vector<Node> children_;
		Mesh* mesh_;
		glm::vec3 translation_{ 0.f, 0.f, 0.f };
		glm::vec3 scale_{ 1.f, 1.f, 1.f };
	};
}

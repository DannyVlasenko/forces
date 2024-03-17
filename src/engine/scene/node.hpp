#pragma once

//stl
#include <span>
#include <string>
#include <vector>
#include <variant>

//glm
#include "gtc/quaternion.hpp"

//forces
#include "camera.hpp"
#include "light.hpp"
#include "mesh_content.hpp"

namespace forces
{
	class Node
	{
	public:
		using Empty = std::monostate;
		using Content = std::variant<Empty, Camera, PointLight, MeshContent>;

		explicit Node(const wchar_t* name) :
			name_(name),
			content_(Empty{})
		{}

		Node(const wchar_t* name, const Camera& camera) :
			name_(name),
			content_(camera)
		{}

		Node(const wchar_t* name, const PointLight& light) :
			name_(name),
			content_(light)
		{}

		Node(const wchar_t* name, const MeshContent& meshContent) :
			name_(name),
			content_(meshContent)
		{}


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

		[[nodiscard]]
		const glm::vec3& scale() const noexcept
		{
			return scale_;
		}

		[[nodiscard]]
		glm::vec3& scale() noexcept
		{
			return scale_;
		}

		[[nodiscard]]
		const glm::quat& rotation() const noexcept
		{
			return rotation_;
		}

		[[nodiscard]]
		glm::quat& rotation() noexcept
		{
			return rotation_;
		}

		[[nodiscard]]
		const Content& content() const noexcept
		{
			return content_;
		}

		[[nodiscard]]
		Content& content() noexcept
		{
			return content_;
		}

		[[nodiscard]]
		std::span<const Node> children() const noexcept
		{
			return children_;
		}

		[[nodiscard]]
		std::span<Node> children() noexcept
		{
			return children_;
		}
		
		[[nodiscard]]
		Node& addChild(Node&& child)
		{
			return children_.emplace_back(std::move(child));
		}

	private:
		std::wstring name_;
		glm::vec3 translation_{ 0.f, 0.f, 0.f };
		glm::vec3 scale_{ 1.f, 1.f, 1.f };
		glm::quat rotation_{ 1.f, 0.f, 0.f, 0.f };
		Content content_;
		std::vector<Node> children_{};
	};

	inline void yaw(Node& node, float grad) noexcept
	{
		node.rotation() *= glm::quat({ 0.0f, glm::radians(grad), 0.0f });
	}

	inline void pitch(Node& node, float grad) noexcept
	{
		node.rotation() *= glm::quat({ glm::radians(grad), 0.0f, 0.0f });
	}

	inline void roll(Node& node, float grad) noexcept
	{
		node.rotation() *= glm::quat({ 0.0f, 0.0f, glm::radians(grad) });
	}

	inline glm::vec3 front(const Node& node) noexcept
	{
		return node.rotation() * glm::vec3{ 0.0f, 0.0f, 1.0f };
	}

	inline glm::vec3 up(const Node& node) noexcept
	{
		return node.rotation() * glm::vec3{ 0.0f, 1.0f, 0.0f };
	}

	inline glm::vec3 right(const Node& node) noexcept
	{
		return node.rotation() * glm::vec3{ 1.0f, 0.0f, 0.0f };
	}
}

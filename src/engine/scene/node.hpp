#pragma once

//stl
#include <string>
#include <vector>

//glm
#include <variant>

#include "gtc/quaternion.hpp"


namespace forces
{
	class EmptyNode;
	class LightNode;
	class CameraNode;
	class MeshNode;

	using ConcreteNode = std::variant<EmptyNode, MeshNode, CameraNode, LightNode>;

	class Node
	{
	public:
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
		const std::vector<ConcreteNode>& children() const noexcept
		{
			return children_;
		}

		[[nodiscard]]
		std::vector<ConcreteNode>& children() noexcept
		{
			return children_;
		}

	protected:
		explicit Node(const wchar_t* name) :
			name_(name)
		{}

	private:
		std::wstring name_;
		glm::vec3 translation_{ 0.f, 0.f, 0.f };
		glm::vec3 scale_{ 1.f, 1.f, 1.f };
		glm::quat rotation_{ 1.f, 0.f, 0.f, 0.f };
		std::vector<ConcreteNode> children_;
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

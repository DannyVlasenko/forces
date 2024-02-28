#include "node.hpp"
#include "mesh_node.hpp"
#include "camera_node.hpp"
#include "light_node.hpp"
#include "empty_node.hpp"

namespace forces
{
	void forces::Node::addChild(ConcreteNode&& child)
	{
		children_.emplace_back(std::move(child));
	}
}

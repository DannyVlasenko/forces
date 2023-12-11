#include "node.h"

#include <assimp/scene.h>

ForcesNode* create_node(ForcesNode* parent, const char* name)
{
	if (parent == nullptr)
	{
		return nullptr;
	}
	auto *node = new aiNode(name);
	reinterpret_cast<aiNode*>(parent)->addChildren(1, &node);
	return reinterpret_cast<ForcesNode*>(node);
}

void delete_node(ForcesNode* node)
{
	const auto *n = reinterpret_cast<aiNode*>(node);
	if (n->mParent == nullptr)
	{
		//RootNode cannot be deleted
		return;
	}
	for (unsigned i = 0; i < n->mNumChildren; ++i)
	{
		delete_node(reinterpret_cast<ForcesNode*>(n->mChildren[i]));
	}

	if (n->mParent->mNumChildren == 1)
	{
		delete[] n->mParent->mChildren;
		n->mParent->mChildren = nullptr;
		n->mParent->mNumChildren = 0;
		delete n;
		return;
	}

	auto **newChildren = new aiNode*[n->mParent->mNumChildren - 1];
	for (unsigned i = 0, k = 0; i < n->mParent->mNumChildren; ++i)
	{
		if (n->mParent->mChildren[i] != n)
		{
			newChildren[k] = n->mParent->mChildren[i];
			++k;
		}
	}
	delete[] n->mParent->mChildren;
	n->mParent->mChildren = newChildren;

	delete n;
}

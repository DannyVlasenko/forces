#include "mesh.hpp"

#include <vec2.hpp>
#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>

using namespace std::string_literals;

namespace opengl
{
    VertexBufferLayout Vertex::layout()
    {
        VertexBufferLayout layout;
        layout.push<float>(3);
        layout.push<float>(3);
        return layout;
    }

    Mesh::Mesh(std::span<Vertex> vertices, std::span<GLuint> indices):
        mVertexBuffer(vertices),
        mIndexBuffer(indices)
    {
		mVertexArray.add_buffer(mVertexBuffer, Vertex::layout());
    }

    void Mesh::draw() const
    {
		mVertexArray.bind();
		mIndexBuffer.bind();
		GLCall(glDrawElements(GL_TRIANGLES, mIndexBuffer.count(), GL_UNSIGNED_INT, nullptr));
    }

    Mesh cube_mesh()
    {
		Vertex vertices[] = {
			//0
            {{-0.5f, -0.5f, 0.5f}, {0.0f, 0.0f, 1.0f}},
			 {{0.5f, -0.5f, 0.5f}, {0.0f, 0.0f, 1.0f}},
			 {{0.5f,  0.5f, 0.5f}, {0.0f, 0.0f, 1.0f}},
			{{-0.5f,  0.5f, 0.5f}, {0.0f, 0.0f, 1.0f}},

			//4
			{{-0.5f, -0.5f, -0.5f}, {0.0f, 0.0f, -1.0f}},
			 {{0.5f, -0.5f, -0.5f}, {0.0f, 0.0f, -1.0f}},
			 {{0.5f,  0.5f, -0.5f}, {0.0f, 0.0f, -1.0f}},
			{{-0.5f,  0.5f, -0.5f}, {0.0f, 0.0f, -1.0f}},

			//8
			{{-0.5f, -0.5f, -0.5f}, {-1.0f, 0.0f, 0.0f}},
			{{-0.5f, -0.5f,  0.5f}, {-1.0f, 0.0f, 0.0f}},
			{{-0.5f,  0.5f,  0.5f}, {-1.0f, 0.0f, 0.0f}},
			{{-0.5f,  0.5f, -0.5f}, {-1.0f, 0.0f, 0.0f}},

			//12
			 {{0.5f, -0.5f, -0.5f}, { 1.0f, 0.0f, 0.0f}},
			 {{0.5f, -0.5f,  0.5f}, { 1.0f, 0.0f, 0.0f}},
			 {{0.5f,  0.5f,  0.5f}, { 1.0f, 0.0f, 0.0f}},
			 {{0.5f,  0.5f, -0.5f}, { 1.0f, 0.0f, 0.0f}},

			 //16
			 {{-0.5f, -0.5f, -0.5f}, {0.0f, -1.0f, 0.0f}},
			  {{0.5f, -0.5f, -0.5f}, {0.0f, -1.0f, 0.0f}},
			  {{0.5f, -0.5f,  0.5f}, {0.0f, -1.0f, 0.0f}},
             {{-0.5f, -0.5f,  0.5f}, {0.0f, -1.0f, 0.0f}},

			 //20
			 {{-0.5f, 0.5f, -0.5f}, {0.0f, 1.0f, 0.0f}},
			  {{0.5f, 0.5f, -0.5f}, {0.0f, 1.0f, 0.0f}},
			  {{0.5f, 0.5f,  0.5f}, {0.0f, 1.0f, 0.0f}},
			 {{-0.5f, 0.5f,  0.5f}, {0.0f, 1.0f, 0.0f}}
		};

		GLuint indices[] =
		{
			 0,  1,  2,  0,  2,  3,
			 4,  7,  6,  4,  6,  5,
			 8,  9, 10,  8, 10, 11,
			12, 15, 14, 12, 14, 13,
			16, 17, 18, 16, 18, 19,
			20, 23, 22, 20, 22, 21
		};	

		return { vertices, indices };
    }
    
    static Mesh processMesh(aiMesh* mesh, const aiScene* scene)
    {
        // data to fill
        std::vector<Vertex> vertices;
        std::vector<unsigned int> indices;

        // walk through each of the mesh's vertices
        for (unsigned int i = 0; i < mesh->mNumVertices; i++)
        {
            Vertex vertex;
            glm::vec3 vector; // we declare a placeholder vector since assimp uses its own vector class that doesn't directly convert to glm's vec3 class so we transfer the data to this placeholder glm::vec3 first.
            // positions
            vector.x = mesh->mVertices[i].x;
            vector.y = mesh->mVertices[i].y;
            vector.z = mesh->mVertices[i].z;
            vertex.position = vector;
            // normals
            if (mesh->HasNormals())
            {
                vector.x = mesh->mNormals[i].x;
                vector.y = mesh->mNormals[i].y;
                vector.z = mesh->mNormals[i].z;
                vertex.normal = vector;
            }

            vertices.push_back(vertex);
        }
        // now wak through each of the mesh's faces (a face is a mesh its triangle) and retrieve the corresponding vertex indices.
        for (unsigned int i = 0; i < mesh->mNumFaces; i++)
        {
            aiFace face = mesh->mFaces[i];
            // retrieve all indices of the face and store them in the indices vector
            for (unsigned int j = 0; j < face.mNumIndices; j++)
                indices.push_back(face.mIndices[j]);
        }
        // process materials
        aiMaterial* material = scene->mMaterials[mesh->mMaterialIndex];
        // we assume a convention for sampler names in the shaders. Each diffuse texture should be named
        // as 'texture_diffuseN' where N is a sequential number ranging from 1 to MAX_SAMPLER_NUMBER. 
        // Same applies to other texture as the following list summarizes:
        // diffuse: texture_diffuseN
        // specular: texture_specularN
        // normal: texture_normalN

        return {vertices, indices};
    }
    
	static void processNode(aiNode* node, const aiScene* scene, std::vector<Mesh> &meshes)
	{
		// process each mesh located at the current node
		for (unsigned int i = 0; i < node->mNumMeshes; i++)
		{
			// the node object only contains indices to index the actual objects in the scene. 
			// the scene contains all the data, node is just to keep stuff organized (like relations between nodes).
			aiMesh* mesh = scene->mMeshes[node->mMeshes[i]];
			meshes.push_back(processMesh(mesh, scene));
		}
		// after we've processed all of the meshes (if any) we then recursively process each of the children nodes
		for (unsigned int i = 0; i < node->mNumChildren; i++)
		{
			processNode(node->mChildren[i], scene, meshes);
		}

	}

    std::vector<Mesh> load_from_file(const std::filesystem::path& file)
    {
		Assimp::Importer import;
		const aiScene* scene = import.ReadFile(file.string(), aiProcess_Triangulate | aiProcess_FlipUVs);

		if (!scene || scene->mFlags & AI_SCENE_FLAGS_INCOMPLETE || !scene->mRootNode)
		{
			throw std::runtime_error("Scene load error: "s + import.GetErrorString());
		}

		std::vector<Mesh> result;
		processNode(scene->mRootNode, scene, result);

		return result;
    }
}

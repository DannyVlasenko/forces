#include "shader.hpp"

#include "glcall.hpp"

namespace opengl
{
	Shader::Shader(const ShaderSource& shader_source):
		Shader(shader_source.type, shader_source.code)
	{}

	Shader::Shader(GLuint type, const char* source)
	{
		GLCall(mId = glCreateShader(type));
		GLCall(glShaderSource(mId, 1, &source, nullptr));
		GLCall(glCompileShader(mId));

		int result;
		GLCall(glGetShaderiv(mId, GL_COMPILE_STATUS, &result));
		if (result == GL_FALSE)
		{
			int length;
			GLCall(glGetShaderiv(mId, GL_INFO_LOG_LENGTH, &length));
			std::string error(length, '\0');
			GLCall(glGetShaderInfoLog(mId, static_cast<GLsizei>(error.length()), &length, error.data()));
			ErrorBehavior.report(error.c_str(), "Shader compiler");
		}
	}

	Shader::Shader(Shader&& other) noexcept:
		mId(other.mId)
	{
		other.mId = 0;
	}

	Shader& Shader::operator=(Shader&& other) noexcept
	{
		std::swap(mId, other.mId);
		return *this;
	}

	Shader::~Shader()
	{
		GLCall(glDeleteShader(mId));
	}

	Program::Program(Program&& other) noexcept :
		mId(other.mId)
	{
		other.mId = 0;
	}

	Program& Program::operator=(Program&& other) noexcept
	{
		std::swap(mId, other.mId);
		return *this;
	}

	Program::~Program()
	{
		GLCall(glDeleteProgram(mId));
	}

	void Program::bind() const
	{
		GLCall(glUseProgram(mId));
	}

	void Program::unbind()
	{
		GLCall(glUseProgram(0));
	}

	void Program::set_uniform_4f(const char* name, GLfloat f0, GLfloat f1, GLfloat f2, GLfloat f3) const
	{
		bind();
		GLCall(const auto location = glGetUniformLocation(mId, name));
		GLCall(glUniform4f(location, f0, f1, f2, f3));
	}

	void Program::set_uniform_mat4(const char* name, glm::mat4 mat) const
	{
		bind();
		GLCall(const auto location = glGetUniformLocation(mId, name));
		glUniformMatrix4fv(location, 1, 0, &mat[0][0]);
	}

	GLuint Program::create_program()
	{
		GLCall(const auto id = glCreateProgram());
		return id;
	}

	void Program::attach(const Shader& shader)
	{
		GLCall(glAttachShader(mId, shader));
	}

	void Program::link_validate() const
	{
		GLCall(glLinkProgram(mId));
		GLCall(glValidateProgram(mId));
	}
}

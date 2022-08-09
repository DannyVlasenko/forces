#ifndef OPENGL_SHADER_HPP
#define OPENGL_SHADER_HPP

#include "glcall.hpp"
#include <iterator>
#include <ranges>

namespace opengl
{
	struct ShaderSource
	{
		GLuint type;
		const char* code;
	};

	class Shader
	{
	public:
		explicit Shader(const ShaderSource &shader_source);
		Shader(GLuint type, const char* source);
		Shader(const Shader& other) = delete;
		Shader& operator=(const Shader& other) = delete;
		Shader(Shader&& other) noexcept;
		Shader& operator=(Shader&& other) noexcept;
		~Shader();

		operator GLuint() const noexcept
		{
			return mId;
		}
		
	private:
		GLuint mId{ 0 };
	};

	constexpr auto shader_view = std::ranges::views::transform([](const auto& source) { return Shader(source); });

	class Program
	{
	public:
		template <std::input_iterator InIt>
		Program(InIt begin, InIt end) :
			Program(std::ranges::subrange(begin, end))
		{}

		template <std::ranges::range ShRng>
		Program(ShRng shaders):
			mId(create_program())
		{			
			for (const auto &shader : shaders) {
				GLCall(glAttachShader(mId, shader));
			}
			link_validate();
		}
		Program(const Program& other) = delete;
		Program& operator=(const Program& other) = delete;
		Program(Program&& other) noexcept;
		Program& operator=(Program&& other) noexcept;
		~Program();

		void bind() const;

		static void unbind();

		void set_uniform_4f(const char* name, GLfloat f0, GLfloat f1, GLfloat f2, GLfloat f3) const;

	private:
		GLuint mId;

		static GLuint create_program();
		void link_validate() const;
	};
}
#endif // OPENGL_SHADER_HPP

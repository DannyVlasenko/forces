#ifndef OPENGL_ERRORS_HPP
#define OPENGL_ERRORS_HPP

#include <stdexcept>

#include "GL/glew.h"

namespace opengl
{
	template <typename T>
	concept ErrorCheckBehavior = requires(T behavior, const char *msg, const char* expr)
	{
		behavior.report(msg, expr);
	};

	class OpenGLError : public std::runtime_error
	{
	public:
		using std::runtime_error::runtime_error;

		static const char* error_msg_from_code(GLenum err) noexcept;

		static void clear();

		template <ErrorCheckBehavior Behaviour>
		static void check(Behaviour &behavior, const char *expr)
		{
			const auto err = glGetError();
			if (err != GL_NO_ERROR)
			{
				behavior.report(error_msg_from_code(err), expr);
			}
		}
	};

	class ThrowErrorBehavior
	{
	public:
		void report(const char* msg, const char* expr);
	};

	class DebugBreakBehavior
	{
	public:
		void report(const char* msg, const char* expr);
	};

#ifdef _DEBUG
	using DefaultErrorBehavior = DebugBreakBehavior;
#else
	using DefaultErrorBehavior = ThrowErrorBehavior;
#endif

	extern DefaultErrorBehavior ErrorBehavior;
}

#endif


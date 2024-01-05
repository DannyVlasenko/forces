#pragma once

namespace forces
{
	class Window
	{
	public:
		virtual ~Window() = default;
		virtual void makeContextCurrent() const = 0;
		virtual void swapBuffers() const = 0;
	};
}

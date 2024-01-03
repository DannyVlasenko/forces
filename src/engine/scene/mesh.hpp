#pragma once
#include <filesystem>

namespace forces
{
	class Mesh
	{
	public:
		Mesh(std::filesystem::path path):
			path_(std::move(path))
		{}

		[[nodiscard]]
		const std::filesystem::path& path() const noexcept
		{
			return path_;
		}

	private:
		std::filesystem::path path_;
	};
}

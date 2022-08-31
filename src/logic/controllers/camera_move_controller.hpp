#ifndef LOGIC_CAMERA_MOVE_CONTROLLER_HPP
#define LOGIC_CAMERA_MOVE_CONTROLLER_HPP

#include "camera.hpp"
#include "glfw/window.hpp"

namespace controllers
{
    class CameraMoveController
    {
    public:
        CameraMoveController(const glfw::Window& window, models::Camera& camera);

        void update();

    private:
        const glfw::Window& mWindow;
        models::Camera& mCamera;
        bool mMouseRightPressed{ false };
        glm::vec2 mLastCursorPos{ 0.0f };
    };
}
#endif // LOGIC_CAMERA_MOVE_CONTROLLER_HPP

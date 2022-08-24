#include "camera_move_controller.hpp"
#include "gtc/matrix_transform.hpp"

namespace controllers
{
    CameraMoveController::CameraMoveController(const glfw::Window& window, models::Camera& camera):
        mWindow(window),
        mCamera(camera)
    {}

    void CameraMoveController::update()
    {
        constexpr float moveSpeed = 0.5f;
        constexpr float rotateSpeed = 0.07f;
        const auto back = normalize(mCamera.translation() - mCamera.look_at());
        const auto right = normalize(cross(mCamera.up(), back));
        const auto up = cross(back, right);
        mCamera.up() = up;

        //WASD strafe 
        if (mWindow.isKeyPressed(GLFW_KEY_W))
        {
            const auto shift = moveSpeed * -back;
            mCamera.translation() += shift;
            mCamera.look_at() += shift;
        }
        if (mWindow.isKeyPressed(GLFW_KEY_S))
        {
            const auto shift = moveSpeed * back;
            mCamera.translation() += shift;
            mCamera.look_at() += shift;
        }
        if (mWindow.isKeyPressed(GLFW_KEY_A))
        {
            const auto shift = moveSpeed * -right;
            mCamera.translation() += shift;
            mCamera.look_at() += shift;
        }
        if (mWindow.isKeyPressed(GLFW_KEY_D))
        {
            const auto shift = moveSpeed * right;
            mCamera.translation() += shift;
            mCamera.look_at() += shift;
        }

        //Arrow keys rotations
        if (mWindow.isKeyPressed(GLFW_KEY_UP))
        {
            const auto axis = right;
            mCamera.look_at() = rotate(glm::mat4(1.f), rotateSpeed, axis) * glm::vec4(mCamera.look_at(), 1.0f);
        }
        if (mWindow.isKeyPressed(GLFW_KEY_DOWN))
        {
            const auto axis = right;
            mCamera.look_at() = rotate(glm::mat4(1.f), -rotateSpeed, axis) * glm::vec4(mCamera.look_at(), 1.0f);
        }
        if (mWindow.isKeyPressed(GLFW_KEY_LEFT))
        {
            const auto axis = up;
            mCamera.look_at() = rotate(glm::mat4(1.f), rotateSpeed, axis) * glm::vec4(mCamera.look_at(), 1.0f);
        }
        if (mWindow.isKeyPressed(GLFW_KEY_RIGHT))
        {
            const auto axis = up;
            mCamera.look_at() = rotate(glm::mat4(1.f), -rotateSpeed, axis) * glm::vec4(mCamera.look_at(), 1.0f);
        }

        //Space/Ctrl up/down
        //Arrow keys rotations
        if (mWindow.isKeyPressed(GLFW_KEY_SPACE))
        {
            const auto shift = moveSpeed * up;
            mCamera.translation() += shift;
            mCamera.look_at() += shift;
        }
        if (mWindow.isKeyPressed(GLFW_KEY_LEFT_CONTROL))
        {
            const auto shift = moveSpeed * -up;
            mCamera.translation() += shift;
            mCamera.look_at() += shift;
        }
    }
}

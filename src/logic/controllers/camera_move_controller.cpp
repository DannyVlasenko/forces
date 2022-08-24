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
        constexpr float moveSpeed = 0.3f;
        constexpr float rotateSpeed = 2.0f;

        //WASD strafe 
        if (mWindow.isKeyPressed(GLFW_KEY_W))
        {
            mCamera.position() += moveSpeed * mCamera.front();
        }
        if (mWindow.isKeyPressed(GLFW_KEY_S))
        {
            mCamera.position() -= moveSpeed * mCamera.front();
        }
        if (mWindow.isKeyPressed(GLFW_KEY_A))
        {
            mCamera.position() += moveSpeed * mCamera.right();
        }
        if (mWindow.isKeyPressed(GLFW_KEY_D))
        {
            mCamera.position() -= moveSpeed * mCamera.right();
        }

        //Arrow keys rotations
        if (mWindow.isKeyPressed(GLFW_KEY_UP))
        {
            mCamera.rotation().x -= rotateSpeed;
        }
        if (mWindow.isKeyPressed(GLFW_KEY_DOWN))
        {
            mCamera.rotation().x += rotateSpeed;
        }
        if (mWindow.isKeyPressed(GLFW_KEY_LEFT))
        {
            mCamera.rotation().y -= rotateSpeed;
        }
        if (mWindow.isKeyPressed(GLFW_KEY_RIGHT))
        {
            mCamera.rotation().y += rotateSpeed;
        }

        //Space/Ctrl up/down
        //Arrow keys rotations
        if (mWindow.isKeyPressed(GLFW_KEY_SPACE))
        {
            mCamera.position() += moveSpeed * mCamera.up();
        }
        if (mWindow.isKeyPressed(GLFW_KEY_LEFT_CONTROL))
        {
            mCamera.position() -= moveSpeed * mCamera.up();
        }
    }
}

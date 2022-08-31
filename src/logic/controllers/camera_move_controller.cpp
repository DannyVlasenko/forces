#include "camera_move_controller.hpp"
#include "gtc/matrix_transform.hpp"
#include "gtx/euler_angles.hpp"

namespace controllers
{
    static void yaw(models::Camera& camera, float grad) 
    {
        const auto rotation = glm::rotate(glm::mat4{ 1.0f }, glm::radians(grad), camera.up());
        glm::vec3 result;
        glm::extractEulerAngleYXZ(rotation, result.y, result.x, result.z);
        camera.rotation() += glm::degrees(result);
    }

    static void pitch(models::Camera& camera, float grad)
    {
        const auto rotation = glm::rotate(glm::mat4{ 1.0f }, glm::radians(grad), camera.right());
        glm::vec3 result;
        glm::extractEulerAngleYXZ(rotation, result.y, result.x, result.z);
        camera.rotation() += glm::degrees(result);
    }

    static void roll(models::Camera& camera, float grad)
    {
        const auto rotation = glm::rotate(glm::mat4{ 1.0f }, glm::radians(grad), camera.front());
        glm::vec3 result;
        glm::extractEulerAngleYXZ(rotation, result.y, result.x, result.z);
        camera.rotation() += glm::degrees(result);
    }

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

        //Arrow, Q, E keys rotations
        if (mWindow.isKeyPressed(GLFW_KEY_UP))
        {
            pitch(mCamera, -rotateSpeed);
        }
        if (mWindow.isKeyPressed(GLFW_KEY_DOWN))
        {
            pitch(mCamera, rotateSpeed);
        }
        if (mWindow.isKeyPressed(GLFW_KEY_LEFT))
        {
            yaw(mCamera, rotateSpeed);
        }
        if (mWindow.isKeyPressed(GLFW_KEY_RIGHT))
        {
            yaw(mCamera, -rotateSpeed);
        }
        if (mWindow.isKeyPressed(GLFW_KEY_Q))
        {
            roll(mCamera, -rotateSpeed);
        }
        if (mWindow.isKeyPressed(GLFW_KEY_E))
        {
            roll(mCamera, rotateSpeed);
        }

        //Space/Ctrl up/down
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

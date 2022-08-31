#include "camera_move_controller.hpp"
#include "gtc/matrix_transform.hpp"
#include "gtx/euler_angles.hpp"

namespace controllers
{
    static glm::vec3 rotation_angles(glm::mat4 rot_mat) 
    {
        glm::vec3 result;
        glm::extractEulerAngleYXZ(rot_mat, result.y, result.x, result.z);
        return glm::degrees(result);
    }

    static void yaw(models::Camera& camera, float grad) 
    {
        const auto rotation = glm::rotate(glm::mat4{ 1.0f }, glm::radians(grad), camera.up());
        camera.rotation() += rotation_angles(rotation);
    }

    static void pitch(models::Camera& camera, float grad)
    {
        const auto rotation = glm::rotate(glm::mat4{ 1.0f }, glm::radians(grad), camera.right());
        camera.rotation() += rotation_angles(rotation);
    }

    static void roll(models::Camera& camera, float grad)
    {
        const auto rotation = glm::rotate(glm::mat4{ 1.0f }, glm::radians(grad), camera.front());
        camera.rotation() += rotation_angles(rotation);
    }

    CameraMoveController::CameraMoveController(const glfw::Window& window, models::Camera& camera):
        mWindow(window),
        mCamera(camera)
    {}

    void CameraMoveController::update()
    {
        constexpr float moveSpeed = 0.3f;
        constexpr float rotateSpeed = 2.0f;

        //WASD strafe Space/Ctrl up/down
        if (mWindow.is_key_pressed(GLFW_KEY_W))
        {
            mCamera.position() += moveSpeed * mCamera.front();
        }
        if (mWindow.is_key_pressed(GLFW_KEY_S))
        {
            mCamera.position() -= moveSpeed * mCamera.front();
        }
        if (mWindow.is_key_pressed(GLFW_KEY_A))
        {
            mCamera.position() += moveSpeed * mCamera.right();
        }
        if (mWindow.is_key_pressed(GLFW_KEY_D))
        {
            mCamera.position() -= moveSpeed * mCamera.right();
        }
        if (mWindow.is_key_pressed(GLFW_KEY_SPACE))
        {
            mCamera.position() += moveSpeed * mCamera.up();
        }
        if (mWindow.is_key_pressed(GLFW_KEY_LEFT_CONTROL))
        {
            mCamera.position() -= moveSpeed * mCamera.up();
        }

        //Arrow, Q, E keys rotations
        if (mWindow.is_key_pressed(GLFW_KEY_UP))
        {
            pitch(mCamera, -rotateSpeed);
        }
        if (mWindow.is_key_pressed(GLFW_KEY_DOWN))
        {
            pitch(mCamera, rotateSpeed);
        }
        if (mWindow.is_key_pressed(GLFW_KEY_LEFT))
        {
            yaw(mCamera, rotateSpeed);
        }
        if (mWindow.is_key_pressed(GLFW_KEY_RIGHT))
        {
            yaw(mCamera, -rotateSpeed);
        }
        if (mWindow.is_key_pressed(GLFW_KEY_Q))
        {
            roll(mCamera, -rotateSpeed);
        }
        if (mWindow.is_key_pressed(GLFW_KEY_E))
        {
            roll(mCamera, rotateSpeed);
        }

        //Mouse move
        if (mWindow.mouse_button_pressed(GLFW_MOUSE_BUTTON_RIGHT))
        {
            const auto cursorPos = mWindow.cursor_position();
            if (!mMouseRightPressed) {
                mWindow.disable_cursor(true);
                mWindow.enable_raw_cursor(true);
                mLastCursorPos = cursorPos;
                mMouseRightPressed = true;
            }
            const auto sens = 0.3f;
            const auto cursorMovement = cursorPos - mLastCursorPos;
            yaw(mCamera, -cursorMovement.x * sens);
            pitch(mCamera, cursorMovement.y * sens);
            mLastCursorPos = cursorPos;
        }
        else
        {
            if (mMouseRightPressed) {
                mWindow.enable_raw_cursor(false);
                mWindow.disable_cursor(false);
                mMouseRightPressed = false;
            }
        }
    }
}

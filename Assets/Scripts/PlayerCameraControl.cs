using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerCameraControl : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public PlayerInput playerInput;
    float xSpeed = 200f;
    float ySpeed = 1f;
    CursorLockMode cachedCursorLockState; 
    bool screenFocused;

    // Start is called before the first frame update
    void Start()
    {
        playerInput.actions.FindActionMap("Camera Control").Enable();
        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
    }

    void Update()
    {
        if(!screenFocused){return;}
        cachedCursorLockState = Cursor.lockState;
    }

    void OnApplicationFocus(bool focusState)
    {
        if (focusState)
        {
            Cursor.lockState = cachedCursorLockState;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    void MoveCamera(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = xSpeed;
            freeLookCamera.m_YAxis.m_MaxSpeed = ySpeed;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (callbackContext.canceled)
        {
            //Effectively 'locks' camera
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void ControlCamera(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Camera Control Action name was: " + callbackContext.action.name);
        switch (callbackContext.action.name)
        {
            case "Mouse Move":
                MoveCamera(callbackContext);      
                break;
            default:
                Debug.LogWarning("Default case in camera control switch statement reached");
                break;
        }
    }
}

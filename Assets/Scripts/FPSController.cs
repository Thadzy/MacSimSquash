using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    public bool canMove = true;
    
    CharacterController characterController;
    
    void Start() 
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update() 
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        // Press Left Shift to run
        bool isRunning = Keyboard.current.leftShiftKey.isPressed;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        
        // Get input for all four movement directions
        float forwardSpeed = canMove ? currentSpeed * (Keyboard.current.wKey.isPressed ? 1 : (Keyboard.current.sKey.isPressed ? -1 : 0)) : 0;
        float rightSpeed = canMove ? currentSpeed * (Keyboard.current.dKey.isPressed ? 1 : (Keyboard.current.aKey.isPressed ? -1 : 0)) : 0;
        
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * forwardSpeed) + (right * rightSpeed);
        #endregion
        
        #region Handles Jumping
        if (Keyboard.current.spaceKey.isPressed && canMove && characterController.isGrounded) 
        {
            moveDirection.y = jumpPower;
        }
        else 
        {
            moveDirection.y = movementDirectionY;
        }
        
        if (!characterController.isGrounded) 
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion
        
        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);
        
        if (canMove) 
        {
            rotationX += -Mouse.current.delta.y.ReadValue() * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Mouse.current.delta.x.ReadValue() * lookSpeed, 0);
        }
        #endregion
    }
}
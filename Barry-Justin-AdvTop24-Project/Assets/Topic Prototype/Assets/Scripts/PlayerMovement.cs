using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*
     * Code inspired by https://www.youtube.com/watch?v=1uW-GbHrtQc&ab_channel=Brogrammer
     * Minus some minor changes, it's virtually the same. 
      */

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float lookXLimit;
    [SerializeField] private float gravity;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        //movement vectors
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float currentSpeedX = canMove ? (walkSpeed) * Input.GetAxis("Vertical") : 0;
        float currentSpeedY = canMove ? (walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        //interpret the vectors and multiply for the direction accordingly
        moveDirection = (forward * currentSpeedX) + (right * currentSpeedY);

        //jump handling
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpStrength; //add to velocity
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        //gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //apply changes
        characterController.Move(moveDirection * Time.deltaTime);

        //mouse movement
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSensitivity;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            mainCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSensitivity, 0);
        }
    }
}

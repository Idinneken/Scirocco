using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    
    public CharacterController controller;
    public Status movementState;

    public LayerMask groundMask;

    public float gravity = -10f;

    internal float walkSpeed = 8f;
    internal float runSpeed = 8f;
    internal float crouchSpeed = 2f;
    internal float currentHorizontalSpeed;

    public float verticalSpeed = 0f;

    public float jumpHeight = 2f;



    void Update()
    {
        #region GATHERING FREE INPUTS

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        bool jump = Input.GetButtonDown("Jump");

        #endregion

        #region GATHERING RULE-BOUND INPUTS

        if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.C) && movementState.currentState != "crouching")
        {
            movementState.ChangeCurrentState("crouching");
            print("changing to crouching");
        }

        if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.C) && movementState.currentState == "crouching")
        {
            movementState.ChangeCurrentState("walking");
            print("changing to walking");
        }


        #endregion

        #region MOVEMENT MODIFIERS

        if (movementState.currentState == "crouching")
        {
            currentHorizontalSpeed = crouchSpeed;
        }

        if (movementState.currentState == "walking")
        {
            currentHorizontalSpeed = walkSpeed;
        }


        #endregion

        #region WASD



        Vector3 strafeAmount = transform.right * x; //Left + Right
        Vector3 forwardAmount = transform.forward * z; //Forward + Back
        Vector3 WASDAmount = forwardAmount + strafeAmount; //L+R + F+B        

        Vector3 WASDMovement = currentHorizontalSpeed * Time.deltaTime * WASDAmount.normalized;
        #endregion

        

        #region JUMPING

        if (controller.isGrounded)
        {
            verticalSpeed = 0f;

            if (jump is true)
            {
                verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jump = false;
            }
        }

        verticalSpeed += gravity * Time.deltaTime;

        #endregion

        Vector3 movement = AdjustVelocityToSlope(new(WASDMovement.x, verticalSpeed * Time.deltaTime, WASDMovement.z));        
        controller.Move(movement);

        //print("isGrounded = " + controller.isGrounded);
    }    

    private Vector3 AdjustVelocityToSlope(Vector3 velocity_)
    {
        var ray = new Ray(transform.position, Vector3.down); //Cast a ray to the floor

        if (Physics.Raycast(ray, out RaycastHit hit, 0.2f)) //If the ray hits
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal); //Get the rotation of the slope 
            var adjustedVelocity = slopeRotation * velocity_; //Adjusted velocity = Rotation of the slope * the velocity

            if (adjustedVelocity.y < 0) //If the y of the velocity is less than 0 (if going down the slope)
            {
                return adjustedVelocity; //Return the calculated velocity
            }            
        }
        return velocity_; //Otherwise, return the velocity already given
    }

}

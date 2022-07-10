using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public EntityState movementState;
    public LayerMask groundMask;

    //Character specific
    float currentHorizontalSpeed, verticalSpeed = 0f;    
    public bool squatting;    
    public float walkSpeed, runSpeed, crouchSpeed, slideSpeed, jumpHeight;
    internal bool walkCondition, runCondition, crouchCondition, slideCondition;

    //Global
    public float gravity = -10f;

    void Update()
    {
        #region GATHERING FREE INPUTS

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        bool jump = Input.GetButtonDown("Jump");

        #endregion

        #region GATHERING RULE-BOUND INPUTS

        if (controller.isGrounded)
        {            
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (movementState.currentState != movementState.potentialStates["sneaking"])
                {
                    movementState.SetState("sneaking");                    
                }
                else
                {
                    movementState.SetState("walking");
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (movementState.currentState != movementState.potentialStates["running"])
                {
                    movementState.SetState("running");
                }
                else
                {
                    movementState.SetState("walking");
                }
            }
        }                        
        #endregion

        Vector3 strafeAmount = transform.right * x; //Left + Right
        Vector3 forwardAmount = transform.forward * z; //Forward + Back
        Vector3 WASDAmount = forwardAmount + strafeAmount; //L+R + F+B        
        Vector3 WASDMovement = currentHorizontalSpeed * Time.deltaTime * WASDAmount.normalized; //WASD Inputs * speed

        #region JUMPING

        if (controller.isGrounded)
        {
            verticalSpeed = 0f;
            if (jump is true)
            {
                verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);                
            }
        }
        verticalSpeed += gravity * Time.deltaTime;

        #endregion

        Vector3 movement = AdjustVelocityToSlope(new(WASDMovement.x, verticalSpeed * Time.deltaTime, WASDMovement.z));
        controller.Move(movement);
    }
    
    public Vector3 AdjustVelocityToSlope(Vector3 velocity_)
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

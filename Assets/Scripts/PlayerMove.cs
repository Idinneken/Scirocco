using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{    
    public CharacterController controller;

    public LayerMask groundMask;

    public float gravity = -10f;

    internal float walkSpeed = 4f;
    internal float runSpeed = 8f;
    internal float crouchSpeed = 2f;
    internal float currentHorizontalSpeed = 12f;

    public float verticalSpeed = 0f;

    public float jumpHeight = 2f;

    
    void Update()
    {
        #region WASD

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 strafeAmount = transform.right * x; //Left + Right
        Vector3 forwardAmount = transform.forward * z; //Forward + Back
        Vector3 WASDAmount = forwardAmount + strafeAmount; //L+R + F+B        

        Vector3 WASDMovement = runSpeed * Time.deltaTime * WASDAmount.normalized;                
        #endregion

        #region JUMPING

        if (controller.isGrounded)
        {
            verticalSpeed = 0f;

            if (Input.GetButtonDown("Jump"))
            {
                verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
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

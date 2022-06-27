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

        Vector3 movement = new(WASDMovement.x, verticalSpeed * Time.deltaTime, WASDMovement.z);
        controller.Move(movement);

        print("isGrounded = " + controller.isGrounded);
    }    

    //private void GroundCheck(Transform GroundChecker_, bool isGrounded_, float groundDistance_, LayerMask groundMask_, Vector3 velocity_)
    //{
    //    isGrounded_ = Physics.CheckSphere(GroundChecker_.position, groundDistance_, groundMask_);

    //    if (isGrounded_ && velocity_.y < 0)
    //    {
    //        velocity_.y = -2f;
    //    }
    //}

    //private void Gravity(CharacterController Controller_, Vector3 velocity_, float gravity_)
    //{
    //    velocity_.y += gravity_ * Time.deltaTime;
    //    Controller_.Move(velocity_ * Time.deltaTime);
    //}

    //private void Jumping(Vector3 velocity_, bool isGrounded_, float gravity_)
    //{
    //    if (Input.GetButtonDown("Jump") && isGrounded_)
    //    {
    //        velocity_.y = Mathf.Sqrt(jumpHeight * -2f * gravity_);
    //    }
    //}

    //private void WASDMovement(CharacterController Controller_, Transform transform_, float speed_)
    //{
    //    float x = Input.GetAxisRaw("Horizontal");
    //    float z = Input.GetAxisRaw("Vertical");
    //    Vector3 move = transform_.right * x + transform_.forward * z;
    //    Controller_.Move(move.normalized * speed_ * Time.deltaTime);
    //}
}

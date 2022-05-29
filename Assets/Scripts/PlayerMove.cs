using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Transform GroundCheck;
    public CharacterController Controller;

    public LayerMask groundMask;
    
    public float gravity = -10f;
    public float groundDistance = 0.4f;
    public float speed = 12f;
    public float jumpHeight = 2f;

    private bool isGrounded;

    Vector3 velocity;
    
    void Update()
    {
        Movement(Controller, speed);


        isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        Jumping();
    }

    private void Jumping()
    {
        
    }

    private void Movement(CharacterController Controller_, float speed_)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        Controller_.Move(move.normalized * speed_ * Time.deltaTime);
    }

    //private void Gravity(CharacterController Controller_, Transform GroundCheck_, Vector3 velocity_, LayerMask groundMask_, float groundDistance_, float gravity_)
    //{
    //    isGrounded = Physics.CheckSphere(GroundCheck_.position, groundDistance_, groundMask_);
    //    if (isGrounded && velocity_.y < 0)
    //    {
    //        velocity_.y = -2f;
    //    }

    //    velocity_.y += gravity_ * Time.deltaTime;
    //    Controller_.Move(velocity_ * Time.deltaTime);
    //}

}

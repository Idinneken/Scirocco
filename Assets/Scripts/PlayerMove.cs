using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");       
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        print(move.normalized);

        controller.Move(move.normalized * speed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Camera PlayerCamera;

    float xRotation = 0f;    
    public float sensitivity = 100f;
    public Vector2 turnAmount;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        turnAmount.x += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        turnAmount.y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= turnAmount.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.localRotation = Quaternion.Euler(0f, turnAmount.x, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}

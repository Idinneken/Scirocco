using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public EntityState movementState;
    public LayerMask groundMask;

    //Character specific
    float currentHorizontalSpeed, verticalSpeed = 0f;        
    public float walkSpeed, runSpeed, crouchSpeed, slideSpeed, jumpHeight;
    public float crouchingColliderHeight, crouchingCameraHeight;
    
    internal float walkingColliderHeight, walkingCameraHeight;    
    
    //Global
    public float gravity = -10f;

    void Awake()
    {
        walkingCameraHeight = GetComponentInChildren<Camera>().transform.localPosition.y;
    }

    void Update()
    {
        #region GATHERING FREE INPUTS

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");        

        #endregion        

        if (controller.isGrounded)
        {            
            if (Input.GetKeyDown(KeyCode.C))
            {
                movementState.ToggleBetweenStates("crouching", "walking");
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                movementState.ToggleBetweenStates("running", "walking");
            }
            
            
        }                        
                
        Movement(x, z);
        Gravity();

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }        
    }    

    private void Crouch()
    {
        Camera camera = GetComponentInChildren<Camera>();
        Vector3 cameraPosition = camera.transform.localPosition;      
        camera.transform.localPosition = new Vector3(cameraPosition.x, crouchingCameraHeight, cameraPosition.z);
    }    

    private void UnCrouch()
    {
        Camera camera = GetComponentInChildren<Camera>();
        Vector3 cameraPosition = camera.transform.localPosition;      
        camera.transform.localPosition = new Vector3(cameraPosition.x, walkingCameraHeight, cameraPosition.z);
    }    

    private void Jump()
    {                
        if (controller.isGrounded)
        {
            verticalSpeed = 0f;
            verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);                         
        }
    }

    private void Gravity()
    {
        if (controller.isGrounded && verticalSpeed <= 0)
        {
            verticalSpeed = 0;
        }
        else if (!controller.isGrounded)
        {
            verticalSpeed += gravity * Time.deltaTime;
        }        
    }

    private void Movement(float x_, float z_)
    {                 
        Vector3 WASDAmount = transform.forward * z_ + transform.right * x_; //L+R + F+B        
        Vector3 WASDMovement = currentHorizontalSpeed * Time.deltaTime * WASDAmount.normalized; //WASD Inputs * speed                

        controller.Move(new(WASDMovement.x, 0f, WASDMovement.z));
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
                // print(adjustedVelocity);
                return adjustedVelocity; //Return the calculated velocity
            }
        }
        return velocity_; //Otherwise, return the velocity already given
    }
}

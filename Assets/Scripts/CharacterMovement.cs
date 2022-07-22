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
    public float crouchingColliderHeight, crouchingCameraHeight;
    
    internal float walkingColliderHeight, walkingCameraHeight;
    
    //Global
    public float gravity = -10f;

    void Awake()
    {
        print("cameraLocalheight" + GetComponentInChildren<Camera>().transform.localPosition);
        print("cameraheight" + GetComponentInChildren<Camera>().transform.position);
        walkingCameraHeight = GetComponentInChildren<Camera>().transform.localPosition.y;
    }

    void Start()
    {
        // print(controller.height);
        // walkingColliderHeight = controller.height;     
        // print("controller height: " + controller.height);
        // print("Collider height: " + walkingColliderHeight);
        // print(walkingColliderHeight);

        

        // print(walkingCameraHeight);
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
            
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }                        
                
        Movement(x, z);
        
    }    

    private void ToggleCrouch()
    {        
        Camera camera = GetComponentInChildren<Camera>();
        Vector3 cameraPosition = camera.transform.localPosition;        

        // print(movementState.stateName);

        if (movementState.stateName != "crouching")
        {   
            // controller.height = walkingColliderHeight;
            // print(cameraPosition);
            // print(camera.transform.position);
            camera.transform.localPosition = new Vector3(cameraPosition.x, walkingCameraHeight, cameraPosition.z);
        }
        else //statename == crouching
        {
            // controller.height = crouchingColliderHeight;
            camera.transform.localPosition = new Vector3(cameraPosition.x, crouchingCameraHeight, cameraPosition.z);
        }
        
    }

    private void Jump()
    {        
        verticalSpeed = 0f;
        verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);                                        
    }

    private void Movement(float x_, float z_)
    {
        verticalSpeed += gravity * Time.deltaTime; //Gravity? Who gives a crap about gravity?

        Vector3 strafeAmount = transform.right * x_; //Left + Right
        Vector3 forwardAmount = transform.forward * z_; //Forward + Back
        Vector3 WASDAmount = forwardAmount + strafeAmount; //L+R + F+B        
        Vector3 WASDMovement = currentHorizontalSpeed * Time.deltaTime * WASDAmount.normalized; //WASD Inputs * speed

        Vector3 movement = /*AdjustVelocityToSlope(*/new(WASDMovement.x, verticalSpeed * Time.deltaTime, WASDMovement.z)/*)*/;
        controller.Move(movement);
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

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;


    //Variables for modifying and storing player movement information
    #region Movement Variables
    public float moveSpeed = 8f; //player walk speed
    public float sprintSpeed = 15f; //player sprint speed
    public float iceSpeedMultiplier = 2f; //player speed on ice
    public float cloudJumpMultiplier = 2f; //player jump height on clouds
    public float jumpForce = 5f; //player jump height
    public float gravity = -40f; //constant gravity acceleration
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isSprinting = false;
    private bool isOnIce = false;
    private bool isOnCloud = false;
    #endregion


    #region Camera Variables
    public Camera cam;
    private float xRotation = 0f;
    private float yRotation = 0f;
    public float xSensitivity = 300f;
    public float ySensitivity = 300f;
    #endregion


    #region Collision Detection
    [SerializeField] private float sphereRadius = 1f; //radius of the spherecast for the groundcheck
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private float ceilingCheckDistance = 2f;
    [SerializeField] private LayerMask jumpableGround;
    public bool isGrounded; //whether or not the player is touching the ground
    public bool isCeiling; //whether or not the player is hitting a ceiling
    #endregion



    private void Start()
    {
        //Get the component references for the controllers
        characterController = GetComponent<CharacterController>();
    }

    //ALL INPUT SHOULD GO HERE
    private void Update()
    {
        //get the mouse input 
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //if the player is on ice or clouds, they can't sprint. Otherwise, they can.
        isSprinting = !isOnIce && !isOnCloud && Input.GetKey(KeyCode.LeftShift);


        //sphere cast from the player's position downwards. If the sphere intersects with the ground, then the player is grounded.
        if (Physics.SphereCast(transform.position, sphereRadius, Vector3.down, out RaycastHit hitInfo, groundCheckDistance, jumpableGround))
        {
            isGrounded = true;
            //check if the player is on ice or clouds
            isOnIce = hitInfo.collider.CompareTag("IceSheet");
            isOnCloud = hitInfo.collider.CompareTag("Cloud");
        }
        else
        {
            isGrounded = false;
            isOnIce = false;
            isOnCloud = false;
        }

        isCeiling = Physics.SphereCast(transform.position, sphereRadius, Vector3.up, out RaycastHit ceilingHitInfo, ceilingCheckDistance);


        HandleLook(mouseInput); //handle camera movement
        PlayerMove(movementInput); //handle movement input

        float currentJumpForce = jumpForce;
        if (isOnCloud)
        {
            currentJumpForce *= cloudJumpMultiplier;
        }
        else
        {
            currentJumpForce = jumpForce;
        }

        //jump if the player is grounded and the space bar is down
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(currentJumpForce * -2f * gravity); //executes jump force with the kinematic equation
        }

        // Reset upward velocity if hitting the ceiling.
        if (isCeiling && velocity.y >= 0)
        {
            Debug.Log("Hit Ceiling");
            velocity.y = 0;
        }
    }

    //ALL PHYSICS STUFF SHOULD GO HERE
    private void FixedUpdate()
    {
        // Reset velocity when grounded to keep it from accumulating
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Check for ceiling collision in FixedUpdate
        isCeiling = Physics.SphereCast(transform.position, sphereRadius, Vector3.up, out RaycastHit ceilingHitInfo, ceilingCheckDistance);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the character
        characterController.Move(velocity * Time.deltaTime);
    }

    public void HandleLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        //horizontal camera movement and makes sure the player rotates
        yRotation += mouseX * Time.deltaTime * xSensitivity;
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);  //rotates the player (and the camera, since it's a child)

        //vertical movement
        xRotation -= mouseY * Time.deltaTime * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); //clamps the rotation
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0); //rotates the camera
    }

    public void PlayerMove(Vector2 input)
    {

        moveInput = input;
        //calculate the move direction
        Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y);

        // Calcuate the player speed based on if they are sprinting or on Ice.
        // Calculate the player height based on if they are on clouds and jump.

        float currentSpeed = moveSpeed;
        float currentJumpForce = jumpForce;

        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }

        if (isOnIce)
        {
            currentSpeed *= iceSpeedMultiplier;
        }

        //move the player
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    public void SetMouseSensitivity(float s)
    {
        xSensitivity = s;
        ySensitivity = s;
    }
}

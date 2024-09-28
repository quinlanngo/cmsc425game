using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;


    //Variables for modifying and storing player movement information
    #region Movement Variables
    public float moveSpeed = 8f; //player walk speed
    public float jumpForce = 3f; //player jump height
    public float gravity = -20f; //constant gravity acceleration
    private Vector2 moveInput;
    private Vector3 velocity;
    #endregion


    #region Camera Variables
    public Camera cam;
    private float xRotation = 0f;
    private float yRotation = 0f;
    public float xSensitivity = 300f;
    public float ySensitivity = 300f;
    #endregion


    #region Collision Detection
    [SerializeField] private float sphereRadius = 0.5f; //radius of the spherecast for the groundcheck
    [SerializeField] private float groundCheckDistance = 1f; 
    public bool isGrounded; //whether or not the player is touching the ground
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

       

        //sphere cast from the player's position downwards. If the sphere intersects with the ground, then the player is grounded.
        isGrounded = Physics.SphereCast(transform.position, sphereRadius, Vector3.down, out RaycastHit hitInfo, groundCheckDistance);

        HandleLook(mouseInput); //handle camera movement
        PlayerMove(movementInput); //handle movement input

        //jump if the player is grounded and the space bar is down
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); //executes jump force with the kinematic equation
        }
    }

    //ALL PHYSICS STUFF SHOULD GO HERE
    private void FixedUpdate()
    {
        //reset velocity when grounded to keep it from accumulating
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //apply gravity
        velocity.y += gravity * Time.deltaTime;
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

        //move the player
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }


}

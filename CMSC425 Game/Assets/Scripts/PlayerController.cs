using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float jumpForce = 1f; 
    private Vector2 moveInput;   
    private bool jumpInput;     

    private CharacterController characterController; 
    private Vector3 velocity;    
    public float gravity = -9.81f;  

    private void Start()
    {
        characterController = GetComponent<CharacterController>();  
    }

    private void FixedUpdate()
    {
        //makes sure velocity doesn't accumulate when grounded by resting it
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  
        }

        //moves player downwards to simulate gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);  

        //applies a small grounding force to make movement smoother
        if (jumpInput && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    
    public void PlayerMove(Vector2 input)
    {
       
        moveInput = input;
        // Create a move direction based on the player's local rotation
        Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

        // Move the player character
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    //Receives the jump and allows to detect a player jump elsewhere
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = true;
        }
        else if (context.canceled)
        {
            jumpInput = false;
        }
    }
    public void PlayerJump(bool isJumping)
    {
        jumpInput = isJumping;  
    }

   
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    

   
}

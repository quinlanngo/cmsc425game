using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Input _playerInput;  
    private Input.PlayerNeutralActions _controlMap;  

    private PlayerController _controller;  
    private PlayerLook _cameraController;

    void Awake()
    {
      
        _playerInput = new Input();
        _controlMap = _playerInput.PlayerNeutral;

      
        _controller = GetComponent<PlayerController>();
        _cameraController = GetComponent<PlayerLook>();


        //listens for jump input events. When the jump button is pressed, it triggers the PlayerJump method with true in the context of the input action (basically, when the context includes the correct input). 
        // When the jump button is released, it triggers PlayerJump with false, using the input context (ctx) to determine the state of the jump action.
        _controlMap.Jump.performed += ctx => _controller.PlayerJump(true);
        _controlMap.Jump.canceled += ctx => _controller.PlayerJump(false);
    }


    void FixedUpdate()
    {
       //reads the inputs and applies their movements
        Vector2 movementInput = _controlMap.Movement.ReadValue<Vector2>();
        Vector2 mouseInput = _controlMap.Look.ReadValue<Vector2>();
        _controller.PlayerMove(movementInput);
        _cameraController.HandleLook(mouseInput);
    }

    //enables the player movement action map when the object is loaded
    private void OnEnable()
    {
        _controlMap.Enable();
    }

    //vice versa
    private void OnDisable()
    {
        _controlMap.Disable();
    }
}

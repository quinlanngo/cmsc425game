using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public float xSensitivity = 20f;
    public float ySensitivity = 20f;

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
}

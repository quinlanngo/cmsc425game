using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float interactDistance = 3f; // Distance to interact with objects
    [SerializeField]
    private LayerMask mask; // Filters the objects that the raycast will hit only to a specific layer
    private PlayerUi playerUi; // Reference to the PlayerUi script
    private InputManager inputManager; // Reference to the InputManager script
    // Start is called before the first frame update
    void Start() {
        cam = GetComponent<PlayerLook>().cam;
        playerUi = GetComponent<PlayerUi>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update() {
        playerUi.updateText(String.Empty); // Clear the text when not interacting with an object
        // Raycast from the camera
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, interactDistance, mask)) {
            Interactable interactable = hitinfo.collider.GetComponent<Interactable>();
            if (interactable != null) { // Check if the object is interactable
                playerUi.updateText(hitinfo.collider.GetComponent<Interactable>().promtmessage); // update the text with the prompt message
                // Inorder to access _controlMap from the InputManager script, I had to make it a public variable
                if (inputManager._controlMap.Interact.triggered) {
                    interactable.BaseInteract();  // trigger the interact method of the interactable object
                }
            }
        }
    }
}

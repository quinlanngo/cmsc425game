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
    private PlayerUi playerUI; // Reference to the PlayerUi script
  
    // Start is called before the first frame update
    void Start() {
        cam = GetComponent<PlayerController>().cam;
        playerUI = GetComponent<PlayerUi>();
    }

    // Update is called once per frame
    void Update() {
        playerUI.updateText(String.Empty); // Clear the text when not interacting with an object
        // Raycast from the camera
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, interactDistance, mask)) {
            IInteractable interactable = hitinfo.collider.GetComponent<IInteractable>();
            if (interactable != null) { // Check if the object is interactable
                playerUI.updateText(interactable.PromptMessage); // update the text with the prompt message
                if (Input.GetKeyDown(KeyCode.E)) {
                    interactable.Interact();  // trigger the interact method of the interactable object
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpController : MonoBehaviour
{
    public Rigidbody rb;
    public Collider col;
    public Transform player, itemContainer, cam;
    public float dropForwardForce = 10f;
    public float dropUpwardForce = 2f;
    
    // List of components to enable/disable when picked up
    public MonoBehaviour[] componentsToToggle;
    
    public bool equipped;
    public static bool slotFull;

    private void Start() {
        SetItemState(equipped);
    }

    private void Update()
    {
        if (equipped && slotFull && Input.GetKeyDown(KeyCode.G)) {
            Drop();
        }
    }

    private void SetItemState(bool isEquipped) {
        // Toggle all specified components
        if (componentsToToggle != null) {
            foreach (var component in componentsToToggle) {
                if (component != null) {
                    component.enabled = isEquipped;
                }
            }
        }

        // Set physics state
        rb.isKinematic = isEquipped;
        col.isTrigger = isEquipped;
        
        // Update global state
        if (isEquipped) {
            slotFull = true;
        }
    }

    public void PickUp() {
        equipped = true;
        
        // Parent to container
        transform.SetParent(itemContainer);
        
        // Reset transform
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        //transform.localScale = Vector3.one;
        
        // Update component states
        SetItemState(true);
    }

    public void Drop() {
        equipped = false;
        slotFull = false;
        
        // Remove parent
        transform.SetParent(null);
        
        // Update component states
        SetItemState(false);
        
        // Add forces
        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);
        
        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
    }
}

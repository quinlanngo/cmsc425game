using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Refrence: https://www.youtube.com/watch?v=Ryi9JxbMCFM&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb

public class pickUpController : MonoBehaviour
{
    public Rigidbody rb;
    public Collider col;
    public Transform player, itemContainer, cam;
    //public float dropForwardForce = 10f;
    //public float dropUpwardForce = 2f;
    
    // List of components to enable/disable when picked up
    public MonoBehaviour[] componentsToToggle;
    
    public bool equipped;

    private void Start() {
        SetItemState(equipped);
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

        // Set rigidbody and collider states
        rb.isKinematic = isEquipped;
        col.isTrigger = isEquipped;
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
/*
    public void Drop() {
        equipped = false;
        slotFull = false;
        
        // Remove parent
        transform.SetParent(null);
        
        // Update component states
        SetItemState(false);
        
        // Add drop force
        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);
        
        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
    }
*/

}

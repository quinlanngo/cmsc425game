using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpController : MonoBehaviour
{
    public GunController gunController;
    public Rigidbody rb;
    public Collider col;
    public Transform player, gunContainer, Cam;
    public float dropForwardForce, dropUpwardForce;
    public bool equipped;
    public static bool slotFull;

    private void Start() {
        if (!equipped) {
            gunController.enabled = false;
            rb.isKinematic = false;
            col.isTrigger = false;
        } if(equipped) {
            gunController.enabled = true;
            rb.isKinematic = true;
            col.isTrigger = true;
            slotFull = true;
        }
    }

    void Update() {
        if (equipped && slotFull && Input.GetKeyDown(KeyCode.G)) {
            Drop();
        }
    }

    public void PickUp() {
        equipped = true;
        slotFull = true;

        // Make the weapon a child of the gunContainer
        transform.SetParent(gunContainer);

        // Reset the position and rotation of the weapon
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        // Make the weapon kinematic and disable the collider
        rb.isKinematic = true;
        col.isTrigger = true;
        gunController.enabled = true;
    } 

    public void Drop() {
        equipped = false;
        slotFull = false;

        // Set the parent to null
        transform.SetParent(null);

        // Make the weapon not kinematic and enable the collider
        rb.isKinematic = false;
        col.isTrigger = false;
        gunController.enabled = false;

        // Add force to the weapon
        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(Cam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(Cam.up * dropUpwardForce, ForceMode.Impulse);
        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
    }

}

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovableObject : ElementalObject
{
    public float launchForce = 20f;    // Force applied to the object
    public float launchAngle = 40f;    // Angle in degrees to launch away
  

    // Method to launch the object at an angle relative to the hit normal
    public void LaunchObjectAway(Vector3 hitPoint, Vector3 hitNormal)
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player object not found! Make sure the Player has the correct tag.");
            return;
        }

        // Recalculate direction from player to the bomb, ignoring the y-axis
        Vector3 playerPosition = player.transform.position;
        Vector3 horizontalDirection = (transform.position - playerPosition);
        horizontalDirection.y = 0; // Ignore vertical component
        horizontalDirection.Normalize();

        Vector3 launchDirection = new Vector3(horizontalDirection.x, Mathf.Sin(Mathf.Deg2Rad * launchAngle), horizontalDirection.z).normalized;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        }
    }


    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Air)
        {
            LaunchObjectAway(hitPoint, hitNormal);
        }
    }
}

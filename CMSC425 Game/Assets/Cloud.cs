using System.Collections.Generic;
using UnityEngine;

public class Cloud : ElementalObject
{
    public float moveSpeed = 2f;         // Speed of movement due to wind
    public float detectionRadius = 1f;  // Radius to check for Wind objects
    private LayerMask windLayerMask;    // Layer mask for Wind objects
    private Vector3 externalForce = Vector3.zero; // Force applied by bullets
    private float externalForceDampening = 5f;    // Dampening factor for force decay

    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal, Vector3 bulletDirection)
    {
        if (element == GunController.Element.Air)
        {
            // Use the hit normal for movement direction
            MoveCloud(bulletDirection);
        }
    }

    private void Start()
    {
        windLayerMask = LayerMask.GetMask("Wind");
    }

    private void Update()
    {
        Vector3 movementDirection = Vector3.zero;

        // Find all colliders within the detection radius that are on the Wind layer
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, windLayerMask);

        foreach (var collider in colliders)
        {
            Wind wind = collider.GetComponent<Wind>();
            if (wind != null && wind.isActiveAndEnabled)
            {
                movementDirection += wind.GetWindDirection();
            }
        }

        // Apply movement based on the cumulative wind direction
        if (movementDirection != Vector3.zero)
        {
            transform.position += moveSpeed * movementDirection.normalized * Time.deltaTime;
        }

        // Apply any external force from air bullets and decay it over time
        if (externalForce != Vector3.zero)
        {
            transform.position += externalForce * Time.deltaTime;
            externalForce = Vector3.Lerp(externalForce, Vector3.zero, externalForceDampening * Time.deltaTime);
        }
    }

    private void MoveCloud(Vector3 direction)
    {
        // Apply a short nudge in the direction
        externalForce += direction.normalized * moveSpeed * 10f;

        // Optional: Log the movement for debugging
        Debug.Log($"Cloud nudged by air bullet in direction: {direction}");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

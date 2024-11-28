using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRadius = 1f; // Radius to check for Wind objects
    private LayerMask windLayerMask;   // Layer mask for Wind objects

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
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

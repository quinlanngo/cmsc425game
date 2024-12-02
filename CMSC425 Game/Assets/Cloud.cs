using System.Collections.Generic;
using UnityEngine;

public class Cloud : ElementalObject
{
    public float moveSpeed = 2f;         // Speed of movement due to wind
    public float detectionRadius = 1f;  // Radius to check for Wind objects
    private LayerMask windLayerMask;    // Layer mask for Wind objects
    private Vector3 externalForce = Vector3.zero; // Force applied by bullets
    private float externalForceDampening = 5f;    // Dampening factor for force decay
    private ParticleSystem particles;

    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal, Vector3 bulletDirection)
    {
        if (element == GunController.Element.Air)
        {
            ApplyBulletForce(bulletDirection);
        }
    }

    private void Start()
    {
        windLayerMask = LayerMask.GetMask("Wind");
        particles = GetComponent<ParticleSystem>();
        Renderer particleRenderer = particles.GetComponent<Renderer>();
        particleRenderer.enabled = true;

    }

    private void Update()
    {
        Vector3 movementDirection = Vector3.zero;

        // Calculate wind force
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, windLayerMask);
        foreach (var collider in colliders)
        {
            Wind wind = collider.GetComponent<Wind>();
            if (wind != null && wind.isActiveAndEnabled)
            {
                movementDirection += wind.GetWindDirection();
            }
        }

        // Apply wind-based movement
        if (movementDirection != Vector3.zero)
        {
            transform.position += moveSpeed * movementDirection.normalized * Time.deltaTime;
        }

        // Apply external bullet force and decay over time
        if (externalForce != Vector3.zero)
        {
            transform.position += externalForce * Time.deltaTime;
            externalForce = Vector3.Lerp(externalForce, Vector3.zero, externalForceDampening * Time.deltaTime);
        }
    }

    private void ApplyBulletForce(Vector3 originalDirection)
    {
        // Get the player's position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player object not found! Make sure the Player has the correct tag.");
            return;
        }

        // Recalculate direction from player to cloud, ignoring the y-axis
        Vector3 playerPosition = player.transform.position;
        Vector3 directionToCloud = transform.position - playerPosition;
        directionToCloud.y = 0; // Ignore y-axis
        directionToCloud = directionToCloud.normalized;

        // Apply bullet force in the recalculated direction
        externalForce += directionToCloud * moveSpeed * 10f;
        Debug.Log($"Cloud hit by bullet, recalculated force direction: {directionToCloud}");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize wind detection radius in the editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

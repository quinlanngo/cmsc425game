using UnityEngine;

public class FireShooter : MonoBehaviour
{
    // Interval between enabling and disabling the fire shooter
    public float fireInterval = 2.0f;

    // Duration that the fire shooter stays active
    public float activeDuration = 1.0f;

    // Controls whether the fire shooter is active
    private bool isActive = false;

    // Reference to the Renderer component of the fire shooter
    private Renderer fireRenderer;

    private void Start()
    {
        // Get the Renderer component attached to this GameObject
        fireRenderer = GetComponent<Renderer>();

        // Start the toggle coroutine
        StartCoroutine(ToggleFire());
    }

    // Coroutine to enable and disable the fire shooter at intervals
    private System.Collections.IEnumerator ToggleFire()
    {
        while (true)
        {
            // Enable the fire shooter
            isActive = true;
            if (fireRenderer != null)
            {
                fireRenderer.enabled = true; // Show the fire
            }
            yield return new WaitForSeconds(activeDuration);

            // Disable the fire shooter
            isActive = false;
            if (fireRenderer != null)
            {
                fireRenderer.enabled = false; // Hide the fire
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }

    // Trigger detection for colliding bombs
    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.gameObject.CompareTag("Bomb"))
        {
            // Get the bomb component and call its Explode method if it exists
            Bomb bomb = other.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.Explode();
            }
        }
    }
}

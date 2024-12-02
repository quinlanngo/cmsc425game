using UnityEngine;

public class FireShooter : MonoBehaviour
{
    // Interval between enabling and disabling the fire shooter
    public float fireInterval = 2.0f;

    // Duration that the fire shooter stays active
    public float activeDuration = 1.0f;

    // Alwasy active
    public bool alwaysActive = false;

    // Controls whether the fire shooter is active
    private bool isActive = false;

    // Reference to the Renderer component of the fire shooter
    private Renderer fireRenderer;
    private ParticleSystem particlesystem;
    
    PlayerHealth health;

    private void Start()
    {
        // Get the Renderer component attached to this GameObject
        fireRenderer = GetComponent<Renderer>();
        particlesystem = GetComponent<ParticleSystem>();
        
        // Start the toggle coroutine
        if(!alwaysActive)
        {
            StartCoroutine(ToggleFire());
        } 
        else
        {
            isActive = true;
        }
        
    }

   
    // Coroutine to enable and disable the fire shooter at intervals
    private System.Collections.IEnumerator ToggleFire()
    {
        AudioSource fireAudioSource = GetComponent<AudioSource>();
        while (true)
        {
            // Enable the fire shooter
            isActive = true;

            if (fireRenderer != null)
            {
                fireRenderer.enabled = true; // Show the fire
            }

            if (particlesystem != null)
            {
                var particleRenderer = particlesystem.GetComponent<Renderer>();
                if (particleRenderer != null)
                {
                    particleRenderer.enabled = true; // Enable the particle renderer
                }
            }
            if (fireAudioSource != null)
            {
                if (!fireAudioSource.isPlaying)
                {
                    fireAudioSource.Play(); // Play the fire sound
                }
            }

            yield return new WaitForSeconds(activeDuration);

            // Disable the fire shooter
            isActive = false;

            if (fireRenderer != null)
            {
                fireRenderer.enabled = false; // Hide the fire
            }

            if (particlesystem != null)
            {
                var particleRenderer = particlesystem.GetComponent<Renderer>();
                if (particleRenderer != null)
                {
                    particleRenderer.enabled = false; // Disable the particle renderer
                }
            }
            if (fireAudioSource != null)
            {
                if (fireAudioSource.isPlaying)
                {
                    fireAudioSource.Stop(); // Stop the fire sound
                }
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

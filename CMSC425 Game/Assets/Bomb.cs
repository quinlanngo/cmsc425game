using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MovableObject
{

    private float armTime = 3f;
    private float explosionRadius = 5f;

    private AudioSource tickingSource;
    private AudioSource explosionSource;
    private bool stopPrime;

    private void Start()
    {
        //get both AudioSource components on this GameObject
        AudioSource[] audioSources = GetComponents<AudioSource>();

        //assign each AudioSource based on the clip name
        foreach (AudioSource source in audioSources)
        {
            if (source.clip != null)
            {
                if (source.clip.name == "ClockTick")
                {
                    tickingSource = source;
                }
                else if (source.clip.name == "BombExplode")
                {
                    explosionSource = source;
                }
            }
        }
    }
    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Fire)
        {
            Debug.Log("Priming");
            StartCoroutine(Prime());
        }

        if (element == GunController.Element.Ice)
        {
            Debug.Log("Freezing");
            StopCoroutine(Prime());
            stopPrime = true;
        }

        if (element == GunController.Element.Air)
        {
            //launching object
            LaunchObjectAway(hitPoint, hitNormal);
        }
    }




    public IEnumerator Prime()
    {
        // Cache the Renderer and ensure it's valid
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red; // Indicate arming with a visual change
        }
        else
        {
            Debug.LogWarning("No Renderer found on Bomb. Skipping color change.");
        }

        // Cache tickingSource and ensure it’s valid
        if (tickingSource == null)
        {
            Debug.LogWarning("Ticking sound source is missing. Bomb will arm silently.");
        }

        // Duration and delay for ticking
        float elapsedTime = 0f;
        float tickInterval = 1f; // Tick every second

        while (elapsedTime < armTime)
        {
            // Check if this game object or component has been destroyed mid-coroutine
            if (this == null || gameObject == null)
            {
                Debug.LogError("Bomb object has been destroyed before the coroutine could complete.");
                yield break;
            }

            // Play ticking sound if the AudioSource is valid
            if (tickingSource != null)
            {
                tickingSource.Play();
            }

            // Wait for the tick interval
            yield return new WaitForSeconds(tickInterval);

            // Increment elapsed time
            elapsedTime += tickInterval;
        }

        // Trigger explosion after arming time is complete
        if (stopPrime)
        {
            stopPrime = false;
            renderer.material.color = Color.black; 
            yield break;
        }
        Explode();
    }

    public void Explode()
    {

        // Create a temporary explosion effect
        GameObject explosionFlash = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        explosionFlash.transform.position = transform.position;
        explosionFlash.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2, explosionRadius * 2); // Scale to match explosion radius
        Renderer renderer = explosionFlash.GetComponent<Renderer>();
        renderer.material.color = Color.yellow;
        Destroy(explosionFlash, 0.1f);

        // Create a separate GameObject for the explosion sound
        GameObject soundObject = new GameObject("ExplosionSound");
        soundObject.transform.position = transform.position;
        AudioSource soundSource = soundObject.AddComponent<AudioSource>();
        soundSource.clip = explosionSource.clip; // Assign the explosion clip
                                                 // Configure spatial sound settings
        soundSource.spatialBlend = 1.0f; // Makes the sound fully 3D
        soundSource.minDistance = 5.0f;  // Start reducing volume after this distance
        soundSource.maxDistance = 10.0f; // Completely silent beyond this distance
        soundSource.rolloffMode = AudioRolloffMode.Logarithmic; // Gradual falloff
        soundSource.Play();
        Destroy(soundObject, soundSource.clip.length); // Destroy after sound completes

        // Detect and affect nearby destructible objects
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in nearbyObjects)
        {
            Destructable dest = nearbyObject.GetComponent<Destructable>();
            PlayerHealth player = nearbyObject.GetComponent<PlayerHealth>();
            Debug.Log("BOOM");

            if (dest != null)
            {
                dest.Destroy();
            }
            if (player != null)
            {
                player.TakeDamage(float.MaxValue);
            }
        }

        // Destroy the bomb object itself
        Destroy(gameObject);
    }


}

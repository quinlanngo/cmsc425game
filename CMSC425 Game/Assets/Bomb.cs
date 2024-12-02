using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MovableObject
{

    private float armTime = 3f;
    private float explosionRadius = 5f;

    [SerializeField] private AudioClip tick;
    [SerializeField] private AudioClip explode;
    [SerializeField] private AudioClip unPrime;
    private bool stopPrime;
    private bool isPrimed;

    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Fire)
        {
            Debug.Log("Priming");
           if (isPrimed)
            {
                Explode();
            } else
            {
                StartCoroutine(Prime());
            }
            
        }

        if (element == GunController.Element.Ice)
        {
            Debug.Log("Freezing");
            if (unPrime != null)
            {
                SFXManager.instance.PlaySFXClip(unPrime, this.transform, 1f);
            }
            
            StopCoroutine(Prime());
            isPrimed = false;
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
        isPrimed = true;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red; // Indicate arming with a visual change
        }
        else
        {
            Debug.LogWarning("No Renderer found on Bomb. Skipping color change.");
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

            SFXManager.instance.PlaySFXClip(tick, this.transform, 1f);

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
        StopCoroutine(Prime());
        // Create a temporary explosion effect
        GameObject explosionFlash = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        explosionFlash.transform.position = transform.position;
        explosionFlash.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2, explosionRadius * 2); // Scale to match explosion radius
        Renderer renderer = explosionFlash.GetComponent<Renderer>();
        renderer.material.color = Color.yellow;
        Destroy(explosionFlash, 0.1f);

        SFXManager.instance.PlaySFXClip(explode, this.transform, 1f);

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

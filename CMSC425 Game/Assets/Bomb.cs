using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MovableObject
{

    private float armTime = 3f;
    private float explosionRadius = 5f;

    private AudioSource tickingSource;
    private AudioSource explosionSource;

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
        if (element == GunController.Element.Air)
        {
            //launching object
            LaunchObjectAway(hitPoint, hitNormal);
        }
    }


    private IEnumerator Prime()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }

        // Duration and delay for ticking
        float elapsedTime = 0f;
        float tickInterval = 1f; // Tick every second

        // Play ticking sound every second until armTime is reached
        while (elapsedTime < armTime)
        {
            
            tickingSource.Play();

            //wait for the tick interval
            yield return new WaitForSeconds(tickInterval);

            //increment elapsed time
            elapsedTime += tickInterval;
        }

        //when arm time ends, trigger the explosion
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
        soundSource.Play();
        Destroy(soundObject, soundSource.clip.length); // Destroy after sound completes

        // Detect and affect nearby destructible objects
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in nearbyObjects)
        {
            Destructable dest = nearbyObject.GetComponent<Destructable>();
            Debug.Log("BOOM");

            if (dest != null)
            {
                dest.Destroy();
            }
        }

        // Destroy the bomb object itself
        Destroy(gameObject);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MovableObject
{

    private float armTime = 3f;
    private float explosionRadius = 5f;
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
        yield return new WaitForSeconds(armTime);
        Explode();

    }

    public void Explode()
    {
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
        
        Destroy(gameObject);
    }


}

using System.Collections;
using UnityEngine;

public class BurnableObject : ElementalObject
{
    public ParticleSystem fireParticles; 
    public float burnDuration = 3f;      //duration for which the object will burn

    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Fire)
        {
            StartCoroutine(Burn());
        }
    }

    //coroutine for burning effect
    private IEnumerator Burn()
    {
        //scale the particle system to match the object's scale
        if (fireParticles != null)
        {
            //match the scale of the fire particles to the scale of the object
            var main = fireParticles.main;
            fireParticles.transform.localScale = transform.localScale;

            //start the fire particle system
            fireParticles.Play();
        }

        //wait for the specified burn duration
        yield return new WaitForSeconds(burnDuration);

        //stop the particles if needed
        if (fireParticles != null)
        {
            fireParticles.Stop();
        }

        //destroy the object after burning
        Destroy(gameObject);
    }
}

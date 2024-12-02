using System.Collections;
using UnityEngine;

public class BurnableObject : ElementalObject
{
    public ParticleSystem fireParticles; 
    public float burnDuration = 3f;      //duration for which the object will burn
    [SerializeField] private AudioClip burn;

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
        SFXManager.instance.PlaySFXClip(burn, this.transform, 1f);
        //scale the particle system to match the object's scale
        if (fireParticles != null)
        {

            Renderer particleRenderer = fireParticles.GetComponent<Renderer>();
            if (particleRenderer != null)
            {
                particleRenderer.enabled = true; // Enable the particle renderer
            }

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

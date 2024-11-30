using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : ElementalObject
{

    //Reference to the ice sheet that is spawned. Is a square since the cylinders colliders are not accurate.
    [SerializeField] private GameObject iceSheetPrefab;
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private AudioClip freeze;

    private GameObject currentCloud;
    private bool cloudIsMoving = false;
    private float moveSpeed = 2f;
    private float cloudHeight = 5f;
   
    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Ice)
        {
            GameObject iceSheet = Freeze(hitPoint, hitNormal);
        }
        if (element == GunController.Element.Fire)
        {
            SpawnCloud(hitPoint);
        }
    }



    private GameObject Freeze(Vector3 hitPoint, Vector3 hitNormal)
    {
        
        //spawns the prefab at the location of the bullet.
        GameObject iceSheet = Instantiate(iceSheetPrefab, hitPoint, Quaternion.identity);

        SFXManager.instance.PlaySFXClip(freeze, iceSheet.transform, 1f);
        // Set iceSheet 's rotation to match the surface normal
        iceSheet.transform.up = transform.up;
        // Set iceSheet parent to be water object
        iceSheet.transform.parent = transform;
        // Set iceSheet position to be at the same height as the water object
        iceSheet.transform.position = new Vector3(iceSheet.transform.position.x, transform.position.y, iceSheet.transform.position.z);
        // Move the iceSheet up to the surface of the water
        iceSheet.transform.localPosition = new Vector3(iceSheet.transform.localPosition.x,
            iceSheet.transform.localPosition.y + 0.5f, iceSheet.transform.localPosition.z);

        Debug.Log("Ice sheet spawned at: " + hitPoint);
        return iceSheet;
    }

    private void SpawnCloud(Vector3 hitPoint)
    {
        currentCloud = Instantiate(cloudPrefab, hitPoint + new Vector3(0, cloudHeight, 0), Quaternion.identity);
    }

}

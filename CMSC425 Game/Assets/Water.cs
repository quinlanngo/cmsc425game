using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : ElementalObject
{

    //Reference to the ice sheet that is spawned. Is a square since the cylinders colliders are not accurate.
    [SerializeField] private GameObject iceSheetPrefab;
    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Ice)
        {
            Freeze(hitPoint, hitNormal);
        }
    }

 

    private void Freeze(Vector3 hitPoint, Vector3 hitNormal)
    {
        //spawns the prefab at the location of the bullet.
        GameObject iceSheet = Instantiate(iceSheetPrefab, hitPoint, Quaternion.identity);
        iceSheet.transform.up = hitNormal;

        Debug.Log("Ice sheet spawned at: " + hitPoint);


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : ElementalObject
{

    //Reference to the ice sheet that is spawned. Is a square since the cylinders colliders are not accurate.
    [SerializeField] private GameObject iceSheetPrefab;
    [SerializeField] private GameObject cloudPrefab;

    private GameObject currentCloud;
    private bool cloudIsMoving = false;
    private float moveSpeed = 2f;
    private float cloudHeight = 3f;
    void Update()
    {
        if (cloudIsMoving) { currentCloud.transform.position += Vector3.up * moveSpeed * Time.deltaTime;}
        if (currentCloud != null && currentCloud.transform.position.y >= transform.position.y + cloudHeight) { cloudIsMoving = false; };
        
    }
    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Ice)
        {
            Freeze(hitPoint, hitNormal);
        }
        if (element == GunController.Element.Fire)
        {
            SpawnCloud(hitPoint);
        }
    }

 

    private void Freeze(Vector3 hitPoint, Vector3 hitNormal)
    {
        //spawns the prefab at the location of the bullet.
        GameObject iceSheet = Instantiate(iceSheetPrefab, hitPoint, Quaternion.identity);
        iceSheet.transform.up = hitNormal;

        Debug.Log("Ice sheet spawned at: " + hitPoint);
    }

    private void SpawnCloud(Vector3 hitPoint)
    {
        //spawns the prefab at the location of the bullet.
        if (currentCloud != null)
        {
            Destroy(currentCloud);
        }
        currentCloud = Instantiate(cloudPrefab, hitPoint, Quaternion.identity);
        cloudIsMoving = true;
    }

}

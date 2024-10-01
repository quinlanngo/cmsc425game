using System.Collections;
using System.Collections.Generic;
using UnityEngine;

{

    
    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Fire)
        {
            Melt(hitPoint, hitNormal);
        }
    }

    private void Melt(Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("Ice Melted");
        Destroy(gameObject);
       
    }
  


}

 

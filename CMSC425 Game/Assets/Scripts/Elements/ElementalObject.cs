using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract base class for objects that can interact with different elements
public abstract class ElementalObject : MonoBehaviour
{
    // Default implementation of interaction with all parameters.
    // Derived classes can override this if they need detailed interaction.
    public virtual void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal, Vector3 bulletDirection)
    {
        InteractWithElement(element, hitPoint, hitNormal);
    }
    // Optional method with fewer parameters (hitPoint and hitNormal)
    public virtual void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {

    }
  
      
 

}

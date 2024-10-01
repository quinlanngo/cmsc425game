using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Abstract base class for objects that can interact with different elements
public abstract class ElementalObject : MonoBehaviour
{

    // Abstract method that must be implemented by derived classes.
    // This method is called when an object is interacted with a specific element.
    // Parameters:
    // - element: The element type (Fire, Ice, Air, Lightning) interacting with the object. This Enum is found in GunController
    // - hitPoint: The point of contact where the bullet hit the elemental object.
    // - hitNormal: The surface normal at the point of interaction.
    public abstract void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal);
}

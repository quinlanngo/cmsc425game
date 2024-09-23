using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // This is the prompt message that will be displayed when the player is near the object
    public string promtmessage; 

    public void BaseInteract() {
        Interact();
    }
    
    protected virtual void Interact() {
        // This is a template method that will be implemented in the subclasses
    }
}

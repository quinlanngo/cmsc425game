using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : IInteractable
{
    void Start()
    {
        Initialize();
    }

    public override void Interact()
    {
        base.Interact();
        Destroy(gameObject);
    }
}

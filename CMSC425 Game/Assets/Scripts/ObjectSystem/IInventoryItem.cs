using UnityEngine;

// Interface for inventory items
public abstract class IInventoryItem : IInteractable {

    private void Awake() {
        Initialize();
    }

    public override void Initialize() {
        base.Initialize();
    }

    public override void Interact() {
        base.Interact();
    }

    public virtual void Use() {
        Debug.Log("Used " + ObjectName);
    }
}
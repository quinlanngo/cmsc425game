using System.Collections.Generic;
using UnityEngine;

// Interface for inventory items
public abstract class IInventoryItem : IInteractable {

    private pickUpController pickUpController;
    private PlayerInventory playerInventory;

    public PlayerInventory Inventory {
        get => playerInventory;
        set => playerInventory = value;
    }

    public pickUpController PickUpC {
        get => pickUpController;
        set => pickUpController = value;
    }

    private void Awake() {
        Initialize();
    }

    public override void Initialize() {
        base.Initialize();
        pickUpController = GetComponent<pickUpController>();
        playerInventory = FindAnyObjectByType<PlayerInventory>();
    }

    public override void Interact() {
        base.Interact();
        if(pickUpController != null) {
            Debug.Log("PickUpController found on " + ObjectName);
            if(pickUpController.equipped == false && playerInventory != null) { 
                Debug.Log("Picking up " + ObjectName);
                playerInventory.AddItem(this);
            } 
        } else {
            Debug.Log("No pickUpController found on " + ObjectName);
        }
    }

    public virtual void Use() {
        Debug.Log("Used " + ObjectName);
    }

    public virtual void AddToInventory() {
        Debug.Log("Added " + ObjectName + " to inventory");
        playerInventory.AddItem(this);
    }

    public virtual int GetItemQuantity() {
       return playerInventory.GetItemQuantity(this);
    }

    public int GetMaxItemQuantity() {
        return playerInventory.GetMaxItemQuantity();
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    // Struct to hold item and its quantity
    [Serializable]
    public struct InventorySlot {
        public IInventoryItem item;
        public int quantity;

        public InventorySlot(IInventoryItem item, int quantity) {
            this.item = item;
            this.quantity = quantity;
        }
    }

    [SerializeField]
    private List<InventorySlot> inventorySlots;
    [SerializeField]
    private int maxItems = 5;  
    [SerializeField]
    private int maxItemQuantity = 10;
    [SerializeField]
    private int currentItem = 0;
    [SerializeField]
    private Transform ItemContainer;

    private void Start() {
        if (inventorySlots == null)
            inventorySlots = new List<InventorySlot>();

        // If there are pre-populated items in the inventory, and incase 
        // They haven't been picked up, pick them up and hide them. 
        // This way another script can pre-populate the inventory with items 
        // without having the player pick them up manually. UseFull for 
        // save/load systems.
        if (inventorySlots.Count > 0) {
            foreach (var slot in inventorySlots) {
                if (slot.item.gameObject.transform.parent != ItemContainer) {
                    var pickUpController = slot.item.GetComponent<pickUpController>();
                    if (pickUpController != null) {
                        pickUpController.PickUp();
                        slot.item.gameObject.SetActive(false);
                    }
                }
            }
            // Set current item to active
            inventorySlots[currentItem].item.gameObject.SetActive(true);
        }
    }

    // Use Mouse Scroll Wheel to switch between items
    public void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            Switch(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            Switch(false);
        }
    }

    public void AddItem(IInventoryItem item) {
        // Find if item already exists in inventory
        var existingSlotIndex = inventorySlots.FindIndex(slot => slot.item.ObjectName == item.ObjectName);

        if (existingSlotIndex != -1) {
            if (inventorySlots[existingSlotIndex].quantity >= maxItemQuantity) {
                Debug.Log("Inventory full - cannot add more of this item");
                return;
            } else {
                // if the Item exists increment quantity
                var slot = inventorySlots[existingSlotIndex];
                slot.quantity++;
                inventorySlots[existingSlotIndex] = slot;
                
                // Destroy the new instance since we just need to increment the count
                Destroy(item.gameObject);
            }
        }
        else {
            // if item does not exist in inventory
            if (inventorySlots.Count < maxItems) {
                // Hide existing items
                foreach (var slot in inventorySlots) {
                    slot.item.gameObject.SetActive(false);
                }

                var pickUpController = item.GetComponent<pickUpController>();
                pickUpController.PickUp();

                // Add new item with quantity 1
                inventorySlots.Add(new InventorySlot(item, 1));
                currentItem = inventorySlots.Count - 1;
            }
            else {
                Debug.Log("Inventory full - cannot add new item type");
                return;
            }
        }
    }

    public void ConsumeItem(IInventoryItem item) {
        var slotIndex = inventorySlots.FindIndex(slot => slot.item == item);
        
        if (slotIndex != -1) {
            var slot = inventorySlots[slotIndex];
            // Reduce quantity of item
            if (slot.quantity <= 1) {
                if (inventorySlots.Count > 0) {
                    inventorySlots.RemoveAt(slotIndex);
                    if(currentItem > inventorySlots.Count - 1) {
                        currentItem = 0;
                    } 
                    Switch(false);
                    
                }
                // Remove the slot entirely if quantity reaches 0
                Destroy(item.gameObject);
                PlayerUi playerUi = GetComponentInParent<PlayerUi>();
                playerUi.UpdateInfoText("", Color.black, Color.white);
            }
            else {
                slot.quantity--;
                // Update the slot with new quantity
                inventorySlots[slotIndex] = slot;
            }
        }
    }

    // The check if the is item being held by the player
    public bool isActive(IInventoryItem item) {
        return inventorySlots[currentItem].item == item;
    }

    // Get the item being held by the player
    public IInventoryItem GetCurrentItem() {
        if (inventorySlots.Count > 0) {
            return inventorySlots[currentItem].item;
        }
        return null;
    }

    // Get the quantity of the item being held by the player
    public int GetCurrentItemQuantity() {
        if (inventorySlots.Count > 0) {
            return inventorySlots[currentItem].quantity;
        }
        return 0;
    }

    // Get quantity of a specific item
    public int GetItemQuantity(IInventoryItem item) {
        var slot = inventorySlots.Find(slot => slot.item == item);
        return slot.quantity;
    }

    // Get the maximum quantity of items that can be held
    public int GetMaxItemQuantity() {
        return maxItemQuantity;
    }

    // Get the nextItem in the inventory
    public IInventoryItem GetNextItem(bool direction) {
        if (inventorySlots.Count > 0) {
            if (direction) {
                currentItem = (currentItem + 1) % inventorySlots.Count;
            }
            else {
                if (currentItem > 0) {
                    currentItem = (currentItem - 1) % inventorySlots.Count;
                }
                else {
                    currentItem = inventorySlots.Count - 1;
                }
            }
            return inventorySlots[currentItem].item;
        }
        return null;
    }

    // Switch between items in the inventory
    public void Switch(bool direction) {
        if (inventorySlots.Count > 0) {
            // Hide current item
            IInventoryItem Item = GetCurrentItem();
            Item.gameObject.SetActive(false);

            // Show next item
            IInventoryItem nextItem = GetNextItem(direction);
            nextItem.gameObject.SetActive(true);
        }
    }
}
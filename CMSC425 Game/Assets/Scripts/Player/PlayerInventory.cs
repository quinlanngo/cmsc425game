using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private List<IInventoryItem> items = new List<IInventoryItem>();
    [SerializeField]
    private int maxItems = 5;
    [SerializeField]
    private int currentItem = 0;
    [SerializeField]
    private Transform itemParent;    

    public void AddItem(IInventoryItem item)
    {
        if (items.Count < maxItems) {
            items.Add(item);
            // set item parent to player
            item.transform.SetParent(itemParent);
            //Destroy(item);
        }
        else {
            Debug.Log("Inventory is full");
        }
    }

    public void RemoveItem(IInventoryItem item)
    {
        if (items.Contains(item)) {
            items.Remove(item);
            Debug.Log("Removed " + item.ObjectName + " from inventory");
        }
    }

    public IInventoryItem GetSelectedItem() {
        return items.Count > 0 ? items[currentItem] : null;
    }

    public IInventoryItem NextItem(bool direction=true) {
        if (items.Count > 0) {
            if (direction) {
                currentItem = currentItem == items.Count - 1 ? 0 : currentItem + 1;
            }
            else {
                currentItem = currentItem == 0 ? items.Count - 1 : currentItem - 1;
            }
            return items[currentItem];
        } else {
            return null;
        }
    }
}

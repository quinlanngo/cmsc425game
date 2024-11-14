using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : IInventoryItem {
    private PlayerHealth playerHealth;
    public float healthToGive = 20;

    public override void Initialize()
    {
        ObjectName = "Health Pack";
        base.Initialize();
    }
    
    // Start is called before the first frame update
    void Start() {
        Initialize();
    }

    // Update is called once per frame
    void Update() {
        PlayerUi playerUi = GetComponentInParent<PlayerUi>();
        playerUi.UpdateInfoText("Heal Strength: " + healthToGive + "\n" 
        + "Quantity: " + GetItemQuantity() + "/" + GetMaxItemQuantity(), Color.black, Color.white);
        playerHealth = GetComponentInParent<PlayerHealth>();
        if(Input.GetKeyDown(KeyCode.H)) {
            Use();
        }
    }

    public override void Use() {
        if(playerHealth.CurrentHealth < playerHealth.MaxHealth && playerHealth.CurrentHealth + healthToGive <= playerHealth.MaxHealth) {
            Inventory.ConsumeItem(this);
            playerHealth.Heal(healthToGive);
        } else {
            Debug.Log("Player is already at Max possible health.");
        }
    }
}

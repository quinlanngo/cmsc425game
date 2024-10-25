using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : IInventoryItem {
    private PlayerHealth playerHealth;
    public float healthToGive = -20;

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
        playerHealth = GetComponentInParent<PlayerHealth>();
        if(Input.GetKeyDown(KeyCode.H)) {
            Use();
        }
    }

    public override void Use() {
        base.Use();
        playerHealth.TakeDamage(healthToGive);
    }
}

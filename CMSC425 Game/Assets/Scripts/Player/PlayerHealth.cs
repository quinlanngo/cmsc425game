using UnityEngine;
using UnityEngine.UI;  

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Maximum player health
    private float currentHealth;   // Current player health
    public Slider healthBar;     // Health bar UI element
    public GameObject DeathText;

    void Start()
    {
        currentHealth = maxHealth;
        DeathText.SetActive(false);
        UpdateHealthBar();
    }

    // Getters and setters
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= maxHealth && currentHealth - damage <= maxHealth) {
            currentHealth -= damage;
        } else if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        } 

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        // Handle player death (e.g., show death screen, disable player controls)
        Debug.Log("Player has died.");
        DeathText.SetActive(true);
        //gameObject.SetActive(false);  // Disable the player
        // RespawnManager.instance.RespawnPlayer();
    }
}

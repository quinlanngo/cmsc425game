using UnityEngine;
using UnityEngine.UI;  

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 1;  // Maximum player health
    private int currentHealth;   // Current player health
    public Slider healthBar;     // Health bar UI element

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

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
        gameObject.SetActive(false);  // Disable the player
        // RespawnManager.instance.RespawnPlayer();
    }
}
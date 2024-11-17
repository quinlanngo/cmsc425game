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

    public void Heal(float amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthBar();
    }
    
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
        {
            Die();
        }
        if (currentHealth <= maxHealth)
        {
            currentHealth -= damage;
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthBar();
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
        Debug.Log("Player has died.");
        DeathText.SetActive(true);

        // Open the death menu via GameMenu script
        GameMenu gameMenu = FindObjectOfType<GameMenu>();
        if (gameMenu != null)
        {
            gameMenu.OpenMenuOnDeath();
        }
    }

}

using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float bulletDamage = 10f;        // Amount of damage the bullet deals
    public float maxTravelDistance = 100f;  // Max distance the bullet can travel before getting destroyed

    private Vector3 initialPosition;        // Store the initial position of the bullet

    void Start()
    {
        // Store the initial position when the bullet is instantiated
        initialPosition = transform.position;
    }

    void Update()
    {
        // Check if the bullet has traveled too far
        float distanceTraveled = Vector3.Distance(initialPosition, transform.position);

        if (distanceTraveled >= maxTravelDistance)
        {
            Destroy(gameObject);  // Destroy the bullet if it has traveled too far
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealth component and apply damage
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                Debug.Log("Player Hit");
                playerHealth.TakeDamage(bulletDamage);
            }

            // Destroy the bullet after it hits the player
            Destroy(gameObject);
        }

        
        else
        {
            Destroy(gameObject);    // Destroy the bullet if it hits anything else (e.g., walls)
        }
    }
}

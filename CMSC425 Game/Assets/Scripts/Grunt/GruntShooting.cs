using UnityEngine;

public class GruntShooting : MonoBehaviour
{
    public GameObject bulletPrefab;     // Bullet object to be instantiated
    public Transform gun;               // Point where the bullets will be spawned from
    public float fireRange = 10f;       // Distance at which the enemy starts shooting
    public float fireRate = 1f;         // Time between shots
    public float bulletSpeed = 20f;     // Speed of the bullets
    public bool canShoot = true;        // Determines if the enemy can shoot
    public bool hasGun = true;          // Determines if the enemy has a gun
    private Transform player;           // Player reference obtained by tag
    private float fireTimer = 0f;       // Timer to track fire rate

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (canShoot && hasGun && player != null)
        {
            ShootIfInRange(player.position);
        }
    }

    public void ShootIfInRange(Vector3 targetPosition)
    {
        float distanceToTarget = Vector3.Distance(gun.position, targetPosition);

        if (distanceToTarget <= fireRange)
        {
            PointGunAtPlayer();
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
                Shoot(targetPosition);
            }
        }
    }

    void Shoot(Vector3 targetPosition)
    {
        GameObject bullet = Instantiate(bulletPrefab, gun.position, gun.rotation);

        Vector3 direction = (targetPosition - gun.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * bulletSpeed;
    }

    void PointGunAtPlayer()
    {
        // Calculate direction from the gun to the player
        Vector3 directionToPlayer = (player.position - gun.position).normalized;
        // Calculate the rotation needed for the gun to face the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        // Smoothly rotate the gun towards the player
        gun.rotation = Quaternion.Slerp(gun.rotation, lookRotation, Time.deltaTime * 5f);
    }
}

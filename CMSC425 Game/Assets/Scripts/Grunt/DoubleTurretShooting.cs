using UnityEngine;

public class DoubleTurretShooting : MonoBehaviour
{
    public Bullet bulletPrefab;          // Bullet object to be instantiated
    public Transform[] gunHoles;         // Array of gun holes (4 total)
    public Transform turretHead;         // The head that rotates towards the player
    public GunController.Element element;
    public float fireRange = 15f;        // Distance at which the enemy starts shooting
    public float fireRate = 1f;          // Time between shots
    public float bulletSpeed = 25f;      // Speed of the bullets
    public int bulletDamage = 15;        // Damage of the bullets
    public bool canShoot = true;         // Determines if the turret can shoot
    public bool hasGuns = true;          // Determines if the turret has guns
    private Transform player;            // Player reference obtained by tag
    private float fireTimer = 0f;        // Timer to track fire rate
    private int currentGunIndex = 0;     // Index to track the current gun for firing

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (canShoot && hasGuns && player != null)
        {
            ShootIfInRange(player.position);
        }
    }

    public void ShootIfInRange(Vector3 targetPosition)
    {
        float distanceToTarget = Vector3.Distance(turretHead.position, targetPosition);

        if (distanceToTarget <= fireRange)
        {
            RotateHeadTowardsPlayer();
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
        // Get the current gun hole for firing
        Transform currentGun = gunHoles[currentGunIndex];
        currentGunIndex = (currentGunIndex + 1) % gunHoles.Length; // Cycle through the gun holes

        // Instantiate the bullet and set its properties
        Bullet bullet = Instantiate(bulletPrefab, currentGun.position, currentGun.rotation);
        bullet.GetComponent<Bullet>().SetDamage(bulletDamage);
        bullet.GetComponent<Bullet>().SetElement(element);
        bullet.GetComponent<Bullet>().AssignMaterial(element);

        // Calculate the direction and apply velocity to the bullet
        Vector3 direction = (targetPosition - currentGun.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * bulletSpeed;
    }

    void RotateHeadTowardsPlayer()
    {
        // Calculate direction from the turret head to the player
        Vector3 directionToPlayer = (player.position - turretHead.position).normalized;
        // Calculate the rotation needed for the turret head to face the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        // Smoothly rotate the turret head towards the player
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, Time.deltaTime * 5f);
    }
}

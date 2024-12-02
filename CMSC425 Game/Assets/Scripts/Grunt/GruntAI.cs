using UnityEngine;
using UnityEngine.AI;

public class GruntAI : MonoBehaviour
{
    public NavMeshAgent agent;               // NavMesh agent for movement
    public Transform[] waypoints;            // Predefined path waypoints
    public float detectionRadius = 10f;      // Radius within which grunt can detect player
    public float health = 100f;              // Enemy health
    public float maxHealth = 100f;           // Enemy max health
    public FloatingHealthBar floatingHealthBar;
    public bool isStationary = false;        // Determines if the enemy is stationary
    public bool canShoot = true;             // Determines if the enemy can shoot (passed to GruntShooting)

    private Transform player;                // Player's transform, found by tag
    private int currentWaypointIndex = 0;
    private bool isChasingPlayer = false;
    private GruntShooting gruntShooting;

    void Start()
    {
        floatingHealthBar.updateHealth(health, maxHealth);
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Reference the GruntShooting component and set shooting capability
        gruntShooting = GetComponent<GruntShooting>();
        if (gruntShooting != null)
        {
            gruntShooting.canShoot = canShoot;
        }

        // Start patrolling if not stationary
        if (!isStationary && waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else if (isStationary)
        {
            agent.isStopped = true; // Stop NavMeshAgent movement
        }
    }

    void Update()
    {
        if (isStationary) return; // Skip movement logic if stationary

        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
            DetectPlayer();
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void DetectPlayer()
    {
        // Check if player is within detection range
        if (player != null && Vector3.Distance(player.position, transform.position) <= detectionRadius)
        {
            isChasingPlayer = true;
            Debug.Log("Player Detected!!");
        }
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        floatingHealthBar.updateHealth(health, maxHealth);
        Debug.Log("Grunt health: " + health);

        if (health <= 0)
        {
            Die();
            EndOfTutorialLevel.gruntsDead  =  EndOfTutorialLevel.gruntsDead + 1;
        }
    }

    private void Die()
    {
        Debug.Log("Grunt is dead!");
        Destroy(gameObject);  // Destroy the enemy GameObject
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>(); // Get Bullet component
            if (bullet != null)
            {
                TakeDamage(bullet.GetDamage()); // Use bullet's damage amount
            }
            Destroy(collision.gameObject);  // Destroy the bullet on impact
        }
    }
}

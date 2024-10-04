using UnityEngine;
using UnityEngine.AI;  // Required for NavMesh Agent

public class GruntAI : MonoBehaviour
{
    public NavMeshAgent agent;  // NavMesh agent for movement
    public Transform[] waypoints;  // Predefined path waypoints
    public float detectionRadius = 10f;  // Radius within which grunt can detect player
    public Transform player;  // Player's transform
    private int currentWaypointIndex = 0;
    private bool isChasingPlayer = false;

    void Start()
    {
        // Start patrolling from the first waypoint
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
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
        // Move between waypoints
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void DetectPlayer()
    {
        // Check if player is within detection range
        if (Vector3.Distance(player.position, transform.position) <= detectionRadius)
        {
            isChasingPlayer = true;
            Debug.Log("Player Detected!!");
        }
    }

    void ChasePlayer()
    {
        // Chase the player
        agent.SetDestination(player.position);
    }
}

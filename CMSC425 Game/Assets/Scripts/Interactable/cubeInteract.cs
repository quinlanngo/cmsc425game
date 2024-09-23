using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeInteract : Interactable
{
    // Drag the obstacle spawner in the following field in the inspector
    [SerializeField] 
    private ObstacleSpawner obstacleSpawner;
    // overrides the Interact method from the Interactable class
    protected override void Interact() {
        obstacleSpawner.SpawnObstacles();
        Debug.Log("Interacting with cube");

    }
}

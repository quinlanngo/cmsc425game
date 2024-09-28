using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubeInteract : IInteractable
{
    
    // Drag the obstacle spawner in the following field in the inspector
    [SerializeField] 
    private ObstacleSpawner obstacleSpawner;

    public void Start()
    {
        Initialize();
    }

    public override void Interact()
    {
        base.Interact();
        obstacleSpawner.SpawnObstacles();
    }
}

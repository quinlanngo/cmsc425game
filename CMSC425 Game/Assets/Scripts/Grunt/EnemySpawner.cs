using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics.Tracing;

public class EnemySpawner : MonoBehaviour
{
    public GameObject gruntPrefab;            // Grunt prefab to spawn
    public List<Transform> spawnPoints;       // List of spawn points for enemies
    public int enemiesPerSpawn = 1;           // Number of enemies to spawn per spawn call
    public float spawnDelay = 0.5f;           // Delay between spawning each enemy
    public GunController.Element element = GunController.Element.Default;

    // Spawns enemies at all specified spawn points
    public void SpawnEnemies()
    {
        if (gruntPrefab == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("Grunt prefab or spawn points are not set.");
            return;
        }

        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                GameObject grunt = Instantiate(gruntPrefab, spawnPoint.position, spawnPoint.rotation);
                grunt.GetComponent<GruntShooting>().element = element;
                yield return new WaitForSeconds(spawnDelay); // Wait between spawns if needed
            }
        }
    }
}

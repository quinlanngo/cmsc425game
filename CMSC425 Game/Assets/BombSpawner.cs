using UnityEngine;
using System.Collections;

public class BombDispenser : MonoBehaviour
{
    // Reference to the bomb prefab
    public GameObject bombPrefab;

    private GameObject currentBomb;

    public float spawnDelay = 2.0f;


    private void Start()
    {
       
        SpawnBomb();
    }

    private void Update()
    {
       
        if (currentBomb == null)
        {
            
            StartCoroutine(SpawnBombWithDelay());
        }
    }

    private IEnumerator SpawnBombWithDelay()
    {
       
        yield return new WaitForSeconds(spawnDelay);

        // Spawn the bomb if it’s still null after the delay
        if (currentBomb == null)
        {
            SpawnBomb();
        }
    }

    private void SpawnBomb()
    {
        // Instantiate the bomb at the specified spawn point or the object's position
        currentBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }
}

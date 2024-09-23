using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;  // The obstacle prefab to spawn
    public int numberOfObstacles = 10; // Number of obstacles to spawn
    public Vector3 baseplateMinBounds; // Minimum bounds of the baseplate
    public Vector3 baseplateMaxBounds; // Maximum bounds of the baseplate
    public float minSize = 1f;       // Minimum height for the obstacle
    public float maxSize = 5f; 


    void Start()
    {
        // disabled for now. press E on the cube to spawn obstacles. 
        // had to make the SpawnObstacles method public
        // SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            float randomX = Random.Range(baseplateMinBounds.x, baseplateMaxBounds.x);
            float randomZ = Random.Range(baseplateMinBounds.z, baseplateMaxBounds.z);
            float randomY = 0f; // Y is fixed at ground level

            float randomHeight = Random.Range(minSize, maxSize);
            float randomWidth = Random.Range(minSize, maxSize);
            float randomLength = Random.Range(minSize, maxSize);

            GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(randomX, randomY + randomHeight /2f, randomZ), Quaternion.identity);
            obstacle.transform.localScale = new Vector3(randomWidth, randomHeight, randomLength); 
        }
    }
}

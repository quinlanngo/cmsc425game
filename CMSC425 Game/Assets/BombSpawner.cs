using UnityEngine;
using System.Collections;

public class BombDispenser : MonoBehaviour
{
    // Reference to the bomb prefab
    public GameObject bombPrefab;

    public GameObject currentBomb;

    public float spawnDelay = 2.0f;
    public bool launchBomb = false;
    public Vector3 launchDirection = Vector3.forward;
    public float launchForce = 10f;


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
        /*
        if (currentBomb != null && Vector3.Distance(currentBomb.transform.position, this.transform.position) > 50f) {
            Bomb bombComponent = currentBomb.GetComponent<Bomb>();
            if (bombComponent != null)
            {
                bombComponent.Explode();

            }
        } */
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
        // Instantiate the bomb prefab
        GameObject instantiatedObject = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        // Attempt to get the Bomb component directly and declare it as such
        if (launchBomb) { 
        Bomb bombComponent = instantiatedObject.GetComponent<Bomb>();
            if (bombComponent != null)
            {
                // If successful, assign to currentBomb and start the coroutine
                currentBomb = instantiatedObject;
                StartCoroutine(bombComponent.Prime());

            }
        } else
        {
            currentBomb = instantiatedObject;
        }

        // Apply launch logic if necessary
        if (launchBomb)
            {
                Rigidbody bombRigidbody = currentBomb.GetComponent<Rigidbody>();
                if (bombRigidbody != null)
                {
                    bombRigidbody.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);
                }
        }
        
    }
    private void OnDrawGizmos()
    {
        // Set gizmo color
        Gizmos.color = Color.red;

        {
            // Draw an arrow representing the launch direction
            if (launchBomb)
            {
                Vector3 start = transform.position;
                Vector3 end = start + launchDirection.normalized * 2.0f; // Scale the arrow for visibility
                Gizmos.DrawLine(start, end);

                // Draw the arrowhead
                Vector3 arrowHeadSize = Vector3.up * 0.2f;
                Gizmos.DrawLine(end, end - (launchDirection.normalized + arrowHeadSize) * 0.2f);
                Gizmos.DrawLine(end, end - (launchDirection.normalized - arrowHeadSize) * 0.2f);
            }
        }
    }
}

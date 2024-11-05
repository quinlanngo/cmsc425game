using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    public Material defaultMaterial;
    public Material litUpMaterial;
    public float lightUpDuration = 0.5f;

    private Renderer targetRenderer;
    private SimonSaysManager simonSaysManager;

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        simonSaysManager = FindObjectOfType<SimonSaysManager>(); // Reference to the game manager
        ResetMaterial();
    }

    // THIS IS A TEST, WE WONT BE USING RAYCASTING FOR BULLETS (its a little dodgy and always doesnt work)
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // On left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Set a maximum distance for the raycast, e.g., 100 units
            float raycastDistance = 100f;

            // Layer mask to specify which layers to interact with (adjust if needed)
            int layerMask = LayerMask.GetMask("Puzzle");

            // Perform the raycast and check if it hits anything within the specified distance
            if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
            {
                // Check if the ray hit this exact target or a child of the target
                if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
                {
                    Debug.Log("Hit puzzle: " + hit.collider.gameObject.name);
                    OnRaycastHit();
                }
                else
                {
                    Debug.Log("Missed target: Ray hit " + hit.collider.gameObject.name);
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any object within range.");
            }
        }
    }

    public void LightUp()
    {
        targetRenderer.material = litUpMaterial;
        Invoke(nameof(ResetMaterial), lightUpDuration); // Reset after a brief moment
    }

    private void ResetMaterial()
    {
        targetRenderer.material = defaultMaterial;
    }

    // This is for gun with Bullet Prefab
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the target was hit by a bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            int targetIndex = transform.GetSiblingIndex(); // Get the index based on sibling order
            simonSaysManager.RegisterPlayerChoice(targetIndex); // Notify the manager of the hit
        }
    }
    // This is for RayCast Gun
    public void OnRaycastHit()
    {
        int targetIndex = transform.GetSiblingIndex();
        simonSaysManager.RegisterPlayerChoice(targetIndex); // Register hit with SimonSaysManager

        // Optional: Flash the target to indicate it was hit
        LightUp();
    }


}

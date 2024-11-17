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

    public void LightUp()
    {
        targetRenderer.material = litUpMaterial;
        Invoke(nameof(ResetMaterial), lightUpDuration); // Reset after a brief moment
    }

    private void ResetMaterial()
    {
        targetRenderer.material = defaultMaterial;
    }

    // When the puzzle is hit by a bullet
    public void puzzleCollision()
    {
        int targetIndex = transform.GetSiblingIndex(); // Get the index based on sibling order
        simonSaysManager.RegisterPlayerChoice(targetIndex); // Notify the manager of the hit
        LightUp(); // Flash the target
    }
}

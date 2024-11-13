using UnityEngine;

public class Checkpoint : IInteractable
{
    private CheckPointManager checkpointManager;
    [SerializeField] private string checkpointID; // Unique ID for this checkpoint


    void Start()
    {
        checkpointManager = FindObjectOfType<CheckPointManager>();
    }

    public override void Interact()
    {
        base.Interact();
        checkpointManager.LoadCheckpoint(checkpointID);
    }
}

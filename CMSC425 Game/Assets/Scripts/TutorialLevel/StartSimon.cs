using UnityEngine;

public class StartSimon : IInteractable
{
    private SimonSaysManager simonSaysManager;

    void Start()
    {
        simonSaysManager = FindObjectOfType<SimonSaysManager>();
    }

    public override void Interact()
    {
        base.Interact();
        simonSaysManager.StartSequence(); // Start the sequence when interacted
    }
}

using UnityEngine;

public class StartSimon : IInteractable
{
    private SimonSaysManager simonSaysManager;
    public GameObject handle;

    void Start()
    {
        simonSaysManager = FindObjectOfType<SimonSaysManager>();
    }

    public override void Interact()
    {
        base.Interact();
        simonSaysManager.StartSequence(); // Start the sequence when interacted
        handle.transform.SetPositionAndRotation(handle.transform.position, Quaternion.Euler(handle.transform.eulerAngles.x, handle.transform.eulerAngles.y, -handle.transform.eulerAngles.z));
    }
}

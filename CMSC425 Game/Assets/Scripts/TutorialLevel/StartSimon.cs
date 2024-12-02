using UnityEngine;

public class StartSimon : IInteractable
{
    private SimonSaysManager simonSaysManager;
    public GameObject handle;
    [SerializeField] AudioClip flip;

    void Start()
    {
        simonSaysManager = FindObjectOfType<SimonSaysManager>();
    }

    public override void Interact()
    {
        base.Interact();
        SFXManager.instance.PlaySFXClip(flip, transform, 1f);
        simonSaysManager.StartSequence(); // Start the sequence when interacted
        handle.transform.SetPositionAndRotation(handle.transform.position, Quaternion.Euler(handle.transform.eulerAngles.x, handle.transform.eulerAngles.y, -handle.transform.eulerAngles.z));
    }
}

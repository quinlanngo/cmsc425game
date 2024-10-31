using UnityEngine;

// Interface for interactable objects
public abstract class IInteractable : IGameObject
{
    [SerializeField]
    private string Message;
    private void Awake() {
        Initialize();
    }

    public virtual void Interact() {
        Debug.Log("Interacted with " + ObjectName);
    }

    public override void Initialize() {
        base.Initialize();
        PromptMessage = "Press E to interact with " + ObjectName;
    }

    public string PromptMessage {
        get => Message;
        set => Message = value;
    }
}
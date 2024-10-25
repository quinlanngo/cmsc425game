using UnityEngine;

// Interface for interactable objects
public abstract class IInteractable : IGameObject
{
    [SerializeField]
    private string Message;
    private pickUpController pickUpController;
    private void Awake() {
        Initialize();
    }

    public virtual void Interact() {
        Debug.Log("Interacted with " + ObjectName);
        if(pickUpController != null) {
            Debug.Log("PickUpController found on " + ObjectName);
            if(pickUpController.equipped == false && pickUpController.slotFull == false) { 
                Debug.Log("Picking up " + ObjectName);
                pickUpController.PickUp();
            }
        } else {
            Debug.Log("No pickUpController found on " + ObjectName);
        }
    }

    public override void Initialize() {
        base.Initialize();
        pickUpController = GetComponent<pickUpController>();
        PromptMessage = "Press E to interact with " + ObjectName;
    }

    public string PromptMessage {
        get => Message;
        set => Message = value;
    }
}
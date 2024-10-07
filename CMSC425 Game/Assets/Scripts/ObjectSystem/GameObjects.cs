using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Base Interface for all Game Objects
public abstract class IGameObject : MonoBehaviour
{
    [SerializeField]
    private string Name;
    public virtual void Initialize() {
        Name = this.name;
        Debug.Log(Name + " Initialized");
    }

    public string ObjectName {
        get => Name;
        set => Name = value;
    }
}

// Interface for interactable objects
public abstract class IInteractable : IGameObject
{
    [SerializeField]
    private string Message;
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

// Interface for inventory items
public abstract class IInventoryItem : IInteractable
{
    public virtual void Use() {
        Debug.Log("Used " + ObjectName);
    }
}
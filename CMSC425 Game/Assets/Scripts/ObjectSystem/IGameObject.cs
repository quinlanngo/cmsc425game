using UnityEngine;

// Base Interface for all Game Objects
public abstract class IGameObject : MonoBehaviour
{
    [SerializeField]
    private string Name;
    public virtual void Initialize() {
        if(Name == null || Name == string.Empty) {
            Name = this.name;
        } else {
            ObjectName = Name;
        }
        Debug.Log(ObjectName + " Initialized");
    }

    public string ObjectName {
        get => Name;
        set => Name = value;
    }
}
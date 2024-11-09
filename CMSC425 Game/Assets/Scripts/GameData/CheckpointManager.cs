using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    private SaveLoadController saveLoadController;

    private void Awake()
    {
        saveLoadController = FindObjectOfType<SaveLoadController>();
    }
    public void LoadCheckpoint(string checkpointID)
    {
        saveLoadController.LoadFromCheckpoint(checkpointID);
        Debug.Log($"Loaded checkpoint {checkpointID}.");
    }
}

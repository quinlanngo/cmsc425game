using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CheckpointData
{
    public string checkpointID; // Unique ID for the checkpoint
    public GameObject triggerObject; // Object that triggers the save
}

public class CheckPointSaver : MonoBehaviour
{
    [SerializeField]
    private List<CheckpointData> checkpoints = new List<CheckpointData>(); // List of checkpoint data

    private HashSet<string> savedCheckpoints = new HashSet<string>(); // To prevent duplicate saves

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.triggerObject != null &&
                    other.bounds.Intersects(checkpoint.triggerObject.GetComponent<Collider>().bounds) &&
                    !savedCheckpoints.Contains(checkpoint.checkpointID))
                {
                    SaveCheckpoint(checkpoint.checkpointID);
                }
            }
        }
    }

    private void SaveCheckpoint(string checkpointID)
    {
        SaveLoadController.Instance.SaveAtCheckpoint(checkpointID);
        Debug.Log($"Checkpoint {checkpointID} saved!");
        savedCheckpoints.Add(checkpointID); // Mark this checkpoint as saved
    }

    private void OnDrawGizmos()
    {
        // Optional: Draw gizmos in the editor for visualizing the trigger objects
        Gizmos.color = Color.cyan;
        foreach (var checkpoint in checkpoints)
        {
            if (checkpoint.triggerObject != null)
            {
                Gizmos.DrawWireCube(
                    checkpoint.triggerObject.transform.position,
                    checkpoint.triggerObject.GetComponent<Collider>().bounds.size
                );
            }
        }
    }
}

using UnityEngine;

public class CheckPointSaver : MonoBehaviour
{
    [SerializeField]
    private string checkpointID; // Unique ID for the checkpoint

    private bool isCheckpointSaved = false; // Prevent duplicate saves

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isCheckpointSaved)
        {
            SaveCheckpoint();
        }
    }

    private void SaveCheckpoint()
    {
        if (!string.IsNullOrEmpty(checkpointID))
        {
            SaveLoadController.Instance.SaveAtCheckpoint(checkpointID);
            Debug.Log($"Checkpoint {checkpointID} saved!");
            isCheckpointSaved = true;
        }
        else
        {
            Debug.LogWarning("Checkpoint ID is not set!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider>().bounds.size);
    }
}
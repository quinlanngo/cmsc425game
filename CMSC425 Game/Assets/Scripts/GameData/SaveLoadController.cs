using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveLoadController : MonoBehaviour
{
    private GameObject playerObject;
    private List<GameObject> inventoryItems = new List<GameObject>();
    private string saveFolderPath;
    private string lastCheckpointFilePath;
    public static SaveLoadController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        saveFolderPath = Path.Combine(Application.persistentDataPath, "Checkpoints");
        lastCheckpointFilePath = Path.Combine(saveFolderPath, "last_checkpoint.json");

        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
    }

    private void InitializeReferences()
    {
        // Locate the player object by tag and retrieve inventory items from PlayerInventory
        playerObject = GameObject.FindGameObjectWithTag("Player");
        inventoryItems.Clear();

        if (playerObject != null)
        {
            var playerInventory = playerObject.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                foreach (var slot in playerInventory.GetInventorySlots())
                {
                    if (slot.item != null)
                    {
                        inventoryItems.Add(slot.item.gameObject);
                    }
                }
            }
        }
    }

    // Save game at a specific checkpoint
    public void SaveAtCheckpoint(string checkpointID)
    {
        InitializeReferences();

        string filePath = Path.Combine(saveFolderPath, $"checkpoint_{checkpointID}.json");
        SavedGameData saveData = new SavedGameData
        {
            playerPosition = playerObject.transform.position.ToArray(),
            playerRotation = playerObject.transform.rotation.ToArray(),
            inventoryItemsData = new List<TransformData>()
        };

        foreach (var item in inventoryItems)
        {
            if (item != null)
            {
                TransformData itemData = new TransformData
                {
                    position = item.transform.position.ToArray(),
                    rotation = item.transform.rotation.ToArray(),
                    scale = item.transform.localScale.ToArray(),
                    name = item.name
                };
                saveData.inventoryItemsData.Add(itemData);
            }
        }

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(filePath, json);
        File.WriteAllText(lastCheckpointFilePath, checkpointID);  // Save last checkpoint ID
        Debug.Log($"Game Saved to: {filePath}");
    }

    // Load the last checkpoint saved
    public void LoadLastCheckpoint()
    {
        if (File.Exists(lastCheckpointFilePath))
        {
            string lastCheckpointID = File.ReadAllText(lastCheckpointFilePath);
            LoadFromCheckpoint(lastCheckpointID);
        }
        else
        {
            Debug.LogWarning("No last checkpoint found.");
        }
    }

    // Load game from a specific checkpoint
    public void LoadFromCheckpoint(string checkpointID)
    {
        string filePath = Path.Combine(saveFolderPath, $"checkpoint_{checkpointID}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var loadedData = JsonConvert.DeserializeObject<SavedGameData>(json);

            // Reload the scene and apply loaded data after a delay
            SceneManager.sceneLoaded += (scene, mode) => StartCoroutine(ApplyLoadedDataWithDelay(loadedData));
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.LogWarning($"No saved data found for checkpoint: {checkpointID}");
        }
    }

    private void ApplyLoadedData(SavedGameData loadData)
    {
        if (playerObject != null)
        {
            CharacterController characterController = playerObject.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterController.enabled = false; // Disable character controller
            }
            playerObject = GameObject.FindGameObjectWithTag("Player");
            playerObject.transform.position = loadData.playerPosition.ToVector3();
            playerObject.transform.rotation = loadData.playerRotation.ToQuaternion();

            Debug.Log("Player: " + playerObject.name + " " + loadData.playerPosition.ToString());
            if (characterController != null)
            {
                characterController.enabled = true; // Re-enable after setting position
            }
        }

        foreach (var itemData in loadData.inventoryItemsData)
        {
            GameObject foundItem = GameObject.Find(itemData.name);
            if (foundItem != null)
            {
                foundItem.transform.position = itemData.position.ToVector3();
                foundItem.transform.rotation = itemData.rotation.ToQuaternion();
                foundItem.transform.localScale = itemData.scale.ToVector3();
            }
        }

        Debug.Log("Game Loaded and applied to scene.");
    }

    private IEnumerator ApplyLoadedDataWithDelay(SavedGameData loadData)
    {
        while (playerObject == null)
        {
            InitializeReferences();
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds until the player is found
        }

        ApplyLoadedData(loadData);
    }
}

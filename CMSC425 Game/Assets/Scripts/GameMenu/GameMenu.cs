using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject menuPanel;  // Panel containing the menu UI
    public Button resumeButton;        // Reference to Resume button
    public TextMeshProUGUI mouseSensitivity;
    public TextMeshProUGUI gameVolume;
    public TextMeshProUGUI sfxVolume;
    public TextMeshProUGUI backgroundVolume;

    private bool isPaused = false;

    private void Start()
    {
        menuPanel.SetActive(false); // Hide menu by default
        gameObject.GetComponent<MenuSelector>().ShowText("");

        mouseSensitivity.text = $"Mouse Sensitivity: 500";
        gameVolume.text = $"Master Volume: 100%";
        gameVolume.text = $"Master Volume: 100%";
        gameVolume.text = $"Master Volume: 100%";

    }

    private void Update()
    {
        // Toggle pause/resume with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

        }
    }
    public void WinGame()
    {
        PauseGame();
        FindObjectOfType<MenuSelector>().ShowText("You WONNN!!");
    }

    public void PauseGame()
    {
        isPaused = true;
        menuPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;  // Pause 
    }

    public void ResumeGame()
    {
        isPaused = false;
        menuPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;  // Resume the game
    }

    public void OpenMenuOnDeath()
    {
        PauseGame();
        resumeButton.interactable = false; // Disable resume button on death
        gameObject.GetComponent<MenuSelector>().ShowText("YOU DIED!!");
    }

    public void GoBackToLastCheckpoint()
    {
        ResumeGame();  // Resume game state
        SaveLoadController.Instance.LoadLastCheckpoint();
    }

    public void QuitGame()
    {
        ResumeGame();  // Resume game state in case we need to confirm quitting
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in Editor
#else
        Application.Quit(); // Quit application
#endif
    }

    public void RemoveAllCheckpoints()
    {
        SaveLoadController.Instance.RemoveAllCheckpoints();
        Debug.Log("All checkpoints removed.");
    }

    public void RemoveCheckpoint(string checkpointID)
    {
        SaveLoadController.Instance.RemoveCheckpoint(checkpointID);
        Debug.Log($"Checkpoint {checkpointID} removed.");
    }

    public void LoadCheckpoint(string checkpointID)
    {
        SaveLoadController.Instance.LoadFromCheckpoint(checkpointID);
        Debug.Log($"Checkpoint {checkpointID} Loaded.");

    }
    public void LoadCheckpoint(Button button)
    {
        // Find the TextMeshPro component in the children of the provided GameObject
        TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();

        if (textComponent != null)
        {
            string checkpointID = textComponent.text; // Use the text as the checkpoint ID
            Debug.Log($"Loading checkpoint: {checkpointID}");
            SaveLoadController.Instance.LoadFromCheckpoint(checkpointID);
            ResumeGame(); // Close the menu after loading the checkpoint
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI component found under the provided GameObject.");
        }
    }

    public void RestartLevel()
    {
        ResumeGame();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }


    public void SetGameVolume(float newVolume)
    {
        AudioListener.volume = Mathf.Clamp01(newVolume / 100f);
        gameVolume.text = "Set Volume: " + newVolume;
        Debug.Log($"Game volume set to: {newVolume}");
    }


    public void SetMouseSensitivity(float newSensitivity)
    {
        // Find the player object and get the PlayerController component
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetMouseSensitivity(newSensitivity);
                mouseSensitivity.text = "Set Mouse Sensitivity: " + newSensitivity;
                Debug.Log($"Mouse sensitivity set to: {newSensitivity}");
            }
            else
            {
                Debug.LogWarning("PlayerController component not found on the Player object.");
            }
        }
        else
        {
            Debug.LogWarning("Player object not found.");
        }
    }

    public void SetBackgroundMusicVolume(float newVolume)
    {
        SoundMixerManager manager = FindObjectOfType<SoundMixerManager>();
        manager.SetMusicLevel(-80f + 0.8f * newVolume);
        backgroundVolume.text = $"Set Background Music Volume: {newVolume}";
        Debug.Log($"Background Music set to: {newVolume}");
    }

    public void SetSFXVolume(float newVolume)
    {

        SoundMixerManager manager = FindObjectOfType<SoundMixerManager>();
        manager.SetSFXLevel(-80f + 0.8f * newVolume);
        sfxVolume.text = $"Set SFX Volume: {newVolume}";
        Debug.Log($"SFX volume set to: {newVolume}");
    }
}


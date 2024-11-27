using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject menuPanel;  // Panel containing the menu UI
    public Button resumeButton;        // Reference to Resume button

    private bool isPaused = false;

    private void Start()
    {
        menuPanel.SetActive(false); // Hide menu by default
    }

    private void Update()
    {
        // Toggle pause/resume with Escape key
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        menuPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;  // Pause the game
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
}

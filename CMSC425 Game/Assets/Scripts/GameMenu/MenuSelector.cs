using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuSelector : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;          // The main menu panel
    public GameObject checkpointsMenu;    // The checkpoints menu under SelectionPanel
    public GameObject optionsMenu;        // The options menu under SelectionPanel
    public GameObject hintMenu;        // The hints menu under SelectionPanel
    public GameObject deathText;          // The death text object

    [Header("Buttons")]
    public Button checkpointsButton;      // Button to open checkpoints menu
    public Button optionsButton;          // Button to open options menu
    public Button hintButton;          // Button to open hint menu

    [Header("Checkpoints")]
    public Transform checkpointsContainer; // Parent container of checkpoint objects

    private void Start()
    {
        // Add listeners for the buttons
        checkpointsButton.onClick.AddListener(ShowCheckpointsMenu);
        optionsButton.onClick.AddListener(ShowOptionsMenu);
        hintButton.onClick.AddListener(ShowHintsMenu);

        // Initially activate the main panel and deactivate others
        ActivateMainPanel();
    }

    public void ActivateMainPanel()
    {
        mainPanel.SetActive(true);
        checkpointsMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void ShowCheckpointsMenu()
    {
        checkpointsMenu.SetActive(true);
        optionsMenu.SetActive(false);
        hintMenu.SetActive(false);


        PopulateCheckpointNames();
    }

    public void ShowOptionsMenu()
    {
        optionsMenu.SetActive(true);
        checkpointsMenu.SetActive(false);
        hintMenu.SetActive(false);
    }
    public void ShowHintsMenu()
    {
        optionsMenu.SetActive(false);
        checkpointsMenu.SetActive(false);
        hintMenu.SetActive(true);
    }

    public void ShowDeathText(bool show)
    {
        deathText.SetActive(show);
    }

    private void PopulateCheckpointNames()
    {
        // Clear and get all checkpoint names from SaveLoadController
        List<string> checkpointNames = SaveLoadController.Instance.GetAllSavedCheckpoints();

        // Get all checkpoint objects under the container
        int index = 0;
        foreach (Transform checkpointTransform in checkpointsContainer)
        {
            // Get the TextMeshPro component of the child
            TextMeshProUGUI checkpointText = checkpointTransform.GetComponentInChildren<TextMeshProUGUI>();

            if (checkpointText != null)
            {
                if (index < checkpointNames.Count)
                {
                    // Assign the checkpoint name
                    checkpointText.text = checkpointNames[index];
                    checkpointTransform.gameObject.SetActive(true); // Make sure it is active
                    index++;
                }
                else
                {
                    // Deactivate unused checkpoint objects
                    checkpointTransform.gameObject.SetActive(false);
                }
            }
        }
    }
}

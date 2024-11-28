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

    [Header("Buttons")]
    public Button checkpointsButton;      // Button to open checkpoints menu
    public Button optionsButton;          // Button to open options menu

    [Header("Checkpoints")]
    public Transform checkpointsContainer; // Parent container of checkpoint objects

    private void Start()
    {
        // Add listeners for the buttons
        checkpointsButton.onClick.AddListener(ShowCheckpointsMenu);
        optionsButton.onClick.AddListener(ShowOptionsMenu);

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

        PopulateCheckpointNames();
    }

    public void ShowOptionsMenu()
    {
        checkpointsMenu.SetActive(false);
        optionsMenu.SetActive(true);
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

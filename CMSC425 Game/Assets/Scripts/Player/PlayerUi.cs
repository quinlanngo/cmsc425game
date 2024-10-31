using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText; // Text to display the score
    [SerializeField]
    private TextMeshProUGUI infoText; // Show how many bullets.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update the text
    public void updateText(string promptMessage) {
        promptText.text = promptMessage;
    }

    public void UpdateInfoText(string info, Color color, Color outlineColor) {
        infoText.SetText(info);
        infoText.faceColor = color;
        infoText.outlineColor = outlineColor;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText; // Text to display the score
    [SerializeField]
    private TextMeshProUGUI infoText; // Text to display the info about item

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update the text
    public void updateText(string promptMessage) {
        promptText.text = promptMessage;
    }

    public void updateInfoText(string info, Color faceColor, Color outLineColor)
    {
        if (infoText != null)
        {
            infoText.SetText(info);
            infoText.faceColor = faceColor;
            infoText.outlineColor = outLineColor;
        }
    }
}

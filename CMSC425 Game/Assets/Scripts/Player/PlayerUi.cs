using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText; // Text to display the score

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update the text
    public void updateText(string promptMessage) {
        promptText.text = promptMessage;
    }

}

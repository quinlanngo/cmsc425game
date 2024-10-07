using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText; // Text to display the score
    [SerializeField]
    private TextMeshProUGUI bullets; // Show how many bullets.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update the text
    public void updateText(string promptMessage) {
        promptText.text = promptMessage;
    }

    public void UpdateAmmoText(int bulletsLeft, int magazineSize) {
        if(bulletsLeft == 0 && magazineSize == 0) {
            bullets.SetText(string.Empty);
        } else {
            bullets.SetText("[" + bulletsLeft + "/" + magazineSize + "]");
        }
    }

}

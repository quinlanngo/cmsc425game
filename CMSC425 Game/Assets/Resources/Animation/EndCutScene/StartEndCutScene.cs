using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndCutScene : MonoBehaviour
{

    public Animator animator;
    public GameObject button;

    private void Update()
    {
        if(button == null)
        {
            StartCoroutine(EndCutScene());
        }
    }

    IEnumerator EndCutScene()
    {
        animator.SetTrigger("EndCutScene");
        yield return new WaitForSeconds(51);
        ShowGameWon();
        
    }

    private void ShowGameWon()
    {
        // Show game won screen
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPuzzle : MonoBehaviour
{

    public GameObject glassCapsule;
    private int[] buttonOrder = new int[5];
    //private Button[] buttons = new Button[5];
 


   

  


    IEnumerator DisplayButtons(int buttonNumber)
    {
        buttonOrder[buttonNumber] = Random.Range(0, 5);

      
        yield return null;

    }


 
}

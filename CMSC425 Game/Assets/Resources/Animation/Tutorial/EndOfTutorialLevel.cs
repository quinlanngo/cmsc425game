using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfTutorialLevel : MonoBehaviour
{
    public static int gruntsDead = 0;
    public Animator animator;
    public GameObject fireBullet;
    public GameObject wall;

    // Update is called once per frame
    void Update()
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(SceneIndex == 0)
        {
            if (gruntsDead >= 4)
            {
                StartCoroutine(EndLevel());
            }
        } if( SceneIndex == 1)
        {
            if(wall == null)
            {
                StartCoroutine(EndLevel());
            }
        } if (SceneIndex == 2)
        {
            if(wall == null)
            {
                StartCoroutine(EndLevel());
            }
        }
    }

    IEnumerator EndLevel()
    {
        animator.SetTrigger("OpenGlass");
        yield return new WaitForSeconds(7);
        Destroy(fireBullet);
    }
}

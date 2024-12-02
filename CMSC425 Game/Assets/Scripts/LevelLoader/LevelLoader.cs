using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public List<GameObject> targets;
    public GameObject player;
    public TextMeshProUGUI loadingText;
    public Animator level;
    public Animator cutScene;
    int SceneIndex;

    private void Start()
    {
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Scene Index: " + SceneIndex);
        if (SceneIndex == 0)
        {
            Debug.Log("Starting CutScene");
            cutScene.SetTrigger("CutScene");
            StartCoroutine(WaitForCutScene(84));
        } else
        {
            SetLoadingText(false, false);
            level.SetTrigger("LevelStarted");
        }
    }

    // Update is called once per frame
    void Update() {

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
            }
        }
        //Debug.Log(targets.Count);
        // if all targets have been destroyed, load the next level
        if (targets.Count == 0 || Input.GetKeyDown(KeyCode.P))
        {
            if(SceneIndex < 3)
            {
                SceneManager.LoadScene(SceneIndex + 1);
            }
            //Debug.Log("Loading next level");
        } 
    }

    IEnumerator WaitForCutScene(int index)
    {
        Debug.Log("Inside function triggering cutscene");
        cutScene.SetTrigger("CutScene");
        Debug.Log("Waiting 84 secods");
        yield return new WaitForSeconds(index);
        Debug.Log("triggering level");
        SetLoadingText(false, false);    
        level.SetTrigger("LevelStarted");
    }

    private string SetLoadingText(bool loading, bool empty)
    {
        Material fireMaterial = Resources.Load<Material>("FireMaterial");
        Material iceMaterial = Resources.Load<Material>("IceMaterial");
        Material airMaterial = Resources.Load<Material>("AirMaterial");
        Color outlineColor = Color.white;
        Color color = Color.white;
        string text = "";

        // if scene index 0 set text color as yellow
        // if scene index 1 set text color as fireMaterial color
        // if scene index 2 set text color as iceMaterial color
        // if scene index 3 set text color as airMaterial color
        switch (SceneIndex)
        {
            case 1:
                color = fireMaterial.color;
                text = "Fire Level";
                break;
            case 2:
                color = iceMaterial.color;
                text = "Ice Level";
                break;
            case 3:
                color = airMaterial.color;
                text = "Air Level";
                break;
            default:
                color = Color.yellow;
                text = "Tutorial Level";
                break;
        }

        if (loading && !empty)
        {
            loadingText.SetText("Loading...");
            loadingText.color = Color.white;
            loadingText.outlineColor = Color.white;
        } else if(!empty)
        {
            loadingText.SetText(text);
            loadingText.color = color;
            loadingText.outlineColor = outlineColor;
        } else if (empty)
        {
            loadingText.SetText("");
        }

        return text;
    }
}

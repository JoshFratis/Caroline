using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text messageText;
    public Image fadeObject;
    
    void Start()
    {
        StartCoroutine(Fade());
    }

    public void CallFadeRestart()
    {
        Debug.Log("StartCoroutine(Fade(true, true));");
        StartCoroutine(Fade(true, true));
    }

    // Fade in or out, optionally restart scene on fade out
    public IEnumerator Fade(bool fadeToBlack = false, bool restartScene = false)
    { 
        // Get color, alpha variable
        Color c = Color.black;
        Color fadeObjectColor = fadeObject.color;
        float fadeObjectAlpha = fadeObjectColor.a;
        
        float increment = -0.01f;
        if (fadeToBlack)
        {
            increment = 0.01f;
        }
        
        // Fade in / out
        for (float a = fadeObjectAlpha; a >= 0 && a <= 1; a += increment) // increment / decrement alpha variable
        {
            c.a = a; // change alhpa of color variable
            fadeObject.color = c; // set fade color to color variable
            yield return null;
        }

        // Reload scene
        if (fadeToBlack && restartScene)
        {
            SceneManager.LoadScene("Cutscene");
        }
        
        StopCoroutine(Fade());
    }
}

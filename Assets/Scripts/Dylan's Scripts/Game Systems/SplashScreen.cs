using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    /// <summary>
    /// Dylan Loe
    /// 10-26-2020
    /// 
    /// Goes to this scene first
    /// 
    /// prompts player to plug in controller and press any button
    /// (then takes them to main menu)
    /// </summary>

    public Text promptText;
    public Color noAlph;
    

    public void MainMenu()
    {
        Debug.Log("start game");
        SceneManager.LoadScene("Shawn_MainMenu");
    }

    IEnumerator beginMainMenuTransition()
    {
        while(promptText.color != noAlph)
        {
            promptText.color = Color.Lerp(promptText.color, noAlph, Mathf.PingPong(Time.time, 1));
        }
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Shawn_MainMenu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
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

    [SerializeField] public PlayerInputActions myActions;

    public PlayerData myPlayerData;
    public TextMeshProUGUI text2;

    public string pluggedIn = "";
    bool _controllerPlugged;

    bool yDir = false;
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-26-2020
    /// 
    /// Starts y movement on button
    /// </summary>
    private void Start()
    {
        var myActions = new PlayerInputActions();
        StartCoroutine(ymove());

    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-26-2020
    /// 
    /// Moves button small increments. Also checks if player has a controller plugged in
    /// and displays accordingly.
    /// </summary>
    private void Update()
    {
        if (yDir)
        {
            transform.localPosition += new Vector3(0, 1.3f * Time.deltaTime, 0);
            transform.localScale += new Vector3(0, 0.08f * Time.deltaTime, 0);
        }
        else
        {
            transform.localPosition += new Vector3(0, -1.3f * Time.deltaTime, 0);
            transform.localScale += new Vector3(0, -0.08f * Time.deltaTime, 0);
        }

        if (Gamepad.current != null)
        {
            pluggedIn = "Press any button on your controller to continue...";
            text2.text = "Press any button on your controller to continue...";
            _controllerPlugged = true;
        }
        else
        {
            pluggedIn = "Please Plug in Controller...";
            text2.text = "Please Plug in Controller...";
            _controllerPlugged = false;
        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-26-2020
    /// 
    /// Loads Main menu when button is pushed.
    /// </summary>
    public void MainMenu()
    {
        Debug.Log("start game");

        myPlayerData.nextSceneStr = "Shawn_MainMenu";
        SceneManager.LoadScene("LoadingScene");
       
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-26-2020
    /// 
    /// Switches direction every increment.
    /// </summary>
    /// <returns></returns>
    IEnumerator ymove()
    {
        yield return new WaitForSeconds(2.0f);
        if (yDir)
            yDir = false;
        else
            yDir = true;

        StartCoroutine(ymove());

    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-26-2020
    /// 
    /// When button pressed, begin fade out animation
    /// </summary>
    public void OnPress()
    {
        if(_controllerPlugged)
            this.GetComponent<Animator>().SetBool("TransStart", true);
    }
}

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


    public GameObject text;
    public TextMeshProUGUI text2;
    Animator anim;

    public string pluggedIn = "";
    bool _controllerPlugged;

    bool yDir = false;
    private void Start()
    {
        var myActions = new PlayerInputActions();
        anim = this.GetComponent<Animator>();
        StartCoroutine(ymove());


        
    }
    private void Update()
    {
        if (yDir)
        {
            transform.localPosition += new Vector3(0, 1.2f * Time.deltaTime, 0);
            transform.localScale += new Vector3(0, 0.07f * Time.deltaTime, 0);
        }
        else
        {
            transform.localPosition += new Vector3(0, 1.2f * Time.deltaTime, 0);
            transform.localScale += new Vector3(0, -0.07f * Time.deltaTime, 0);
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


    public void MainMenu()
    {
        Debug.Log("start game");
        SceneManager.LoadScene("Shawn_MainMenu");
       //StartCoroutine(beginM)
    }

    IEnumerator ymove()
    {
        yield return new WaitForSeconds(2.0f);
        if (yDir)
            yDir = false;
        else
            yDir = true;

        StartCoroutine(ymove());

    }

    public void OnPress()
    {
        if(_controllerPlugged)
            this.GetComponent<Animator>().SetBool("TransStart", true);
        //anim.Play("SplashFadeOut");
    }
}

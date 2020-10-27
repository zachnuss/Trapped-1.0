﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

    public void MainMenu()
    {
        SceneManager.LoadScene("Shawn_MainMenu");
    }
}

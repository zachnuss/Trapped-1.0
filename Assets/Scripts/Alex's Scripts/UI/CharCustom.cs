using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharCustom : MonoBehaviour
{
    public PlayerData playerData;
    
    //All the images needed to swap to during gameplay
    public Image Guard;
    public Image Prisoner;
    public Image Korben;
    public Image Lazurus;
    public Image Merlon;

    //All the Sets of colors and models used in the scene
    public GameObject maleColor;
    public GameObject femaleColor;
    public GameObject maleColor1;
    public GameObject femaleColor1;
    public GameObject players;
    public GameObject pets;

    //Bools to keep track of what is active
    private bool playerActive = true;

    /// <summary>
    /// Alexander
    /// Updated: 11-6-2020
    /// 
    /// Function to switch between the character and the pets.
    /// </summary>
    public void switchToPets()
    {
        //Checks to see if the characters are active and then switch to the pets
        if(playerActive == true)
        {
            playerActive = false;
            players.SetActive(false);
            Prisoner.enabled = false;
            pets.SetActive(true);
        }
    }

    /// <summary>
    /// Alexander
    /// Updated: 11-6-2020
    /// 
    /// Function to switch between the character and the pets.
    /// </summary>
    public void switchCharacter()
    {
        //Checks to see if characters is not active then turns them on
        if (playerActive == false)
        {
            playerActive = true;
            pets.SetActive(false);
            Prisoner.enabled = true;
            players.SetActive(true);
        }
    }

    /// <summary>
    /// Alexander
    /// Updated: 11-6-2020
    /// 
    /// Function to switch to Weapons
    /// </summary>
    public void switchToWeapons()
    {
        Debug.Log("Show me the WEAPONS!");
    }

    /// <summary>
    /// Alexander
    /// Updated: 11-6-2020
    /// 
    /// When Color button is pressed it swtiches the input to the color sections
    /// </summary>
    public void ColorBox()
    {
        //Checks to see if the prisoner is active
        if (playerActive == true) //If it is then it will go to colors
        {
            //Uses the event system to set the buttons for the UI
            EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
            EventSystem.current.SetSelectedGameObject(maleColor1); //Sets the selected obj
        }
    }

    /// <summary>
    /// Alexander
    /// Updated: 11-6-2020
    /// 
    /// Two Functions that have the same purpose
    /// Switches models based on right or left bumper being pressed
    /// </summary>
    public void OnRB()
    {
        //Checks to see if the prisoner is active
        if (playerActive == true) //If it is then it will go and adjust the players colors
        {
            playerData.SetCharacterChoiceMenu();
        }
    }
    public void OnLB()
    {
        //Checks to see if the prisoner is active
        if (playerActive == true) //If it is then it will go and adjust the players colors
        {
            playerData.SetCharacterChoiceMenu();
        }
    }

}

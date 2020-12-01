using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievements : MonoBehaviour
{
    public PlayerData playerdata;
    
    //Public fields to input the Achivement description fields to be adjusted with achievements.
    public Text achievementTitle;
    public Text achievementDescription;
    public Text achivementComplete;

    //Private text variable linked to the achievement descriptions for the game objects
    public Text description;


    /// <summary>
    /// Alexander
    /// Updated: 11-17-2020
    /// 
    /// Start function to disable the complete text at the beginning of the scene
    /// </summary>
    private void Start()
    {
        achivementComplete.enabled = false;
    }


    /// <summary>
    /// Alexander
    /// Updated: 11-17-2020
    /// 
    /// Function to go through all the achievemnt checker functions made by Wesley in Player Data in a bool and if true is returned then enable the complete text.
    /// </summary>
    private bool CheckAchievement()
    {
        bool result = false; //Variable to keep track of the bool

        //**Tester**
        Debug.Log("This is the achievment: " + gameObject.name);

        //Set of if-else statements to track the achievements
        if (gameObject.name == "First Timer")
        {
            result = playerdata.achievementFirstTimer;
        }
        else if (gameObject.name == "We got a Runner!")
        {
            result = playerdata.achievementRunner;
        }
        else if (gameObject.name == "Vaccum Murderer")
        {
            result = playerdata.achievementVacuumMurderer;
        }
        else if (gameObject.name == "Frequent Jail Bird")
        {
            result = playerdata.achievementJailBird;
        }
        else if (gameObject.name == "Vengence 1")
        {
            result = playerdata.achievementRevenge1;
        }
        else if (gameObject.name == "Vengence 2")
        {
            result = playerdata.achievementRevenge2;
        }
        else if (gameObject.name == "Does it End?")
        {
            result = playerdata.achievementDoesItEnd;
        }
        else if (gameObject.name == "Don't Trust Anyone")
        { 
            result = playerdata.achievementNoTrust;
        }
        else if (gameObject.name == "Full Prison Wallet")
        {
            result = playerdata.achievementFullWallet;
        }
        else if (gameObject.name == "Red Means Dead")
        {
            result = playerdata.achievementRedDead;
        }

        return result;
    }

    /// <summary>
    /// Alexander
    /// Updated: 11-17-2020
    /// 
    /// Function for when the Controller hovers over a button and checks to see if the achievement was completed
    /// </summary>
    public void Selected()
    {
        achievementTitle.text = gameObject.name;
        achievementDescription.text = description.text;
        //Check to see if the achievement was completed
        if(CheckAchievement() == true)
        {
            achivementComplete.enabled = true;
        }
    }

    /// <summary>
    /// Alexander
    /// Updated: 11-17-2020
    /// 
    /// Function for when the Controller leaves a button and disables the complete text if it was active
    /// </summary>
    public void Deselected()
    {
        //If the achivement completed text was active then disable it
        if(achivementComplete.enabled == true)
            achivementComplete.enabled = false;
    }
}
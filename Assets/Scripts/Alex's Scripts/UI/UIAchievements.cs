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

    //Start function is called before the first frame update.
    private void Start()
    {
        achivementComplete.enabled = false;
    }

    //Function for when the Controller overs over a button
    public void Selected()
    {
        achievementTitle.text = gameObject.name;
        achievementDescription.text = description.text;
        //achivementComplete.enabled = true; //AHL : TEST!
        Debug.Log("Controller is over " + gameObject.name + ".");
    }

    //Function for when the Controller leaves a button
    public void Deselected()
    {
        //achivementComplete.enabled = false; //AHL : TEST!
        Debug.Log("Controller is no longer over " + gameObject.name + ".");
    }
}
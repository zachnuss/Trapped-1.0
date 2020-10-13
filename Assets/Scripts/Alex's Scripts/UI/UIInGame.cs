﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    //Gets access from the PlayerData to keep track of total currency owned
    public PlayerData playerData;
    public PlayerMovement currentPlayer;

    //Text Variables to change the text of the On-Screen Items
    public Text currencyText;
    public Text healthText;

    //Image Variable to change the health bar to match the missing health percentage
    public Image healthBar; //Links to hp bar Image
    public float currHealth = 0f; //Current health of the player
    public float hpBarX = 0f; //X Scale of the image bar to be set later

    public Text objectiveText;
    public int objectiveTracker;

    //Cube and Loop progression Variables
    public Text loopsText;  //Links to loop num text for UI
    private int loopsCompleted = 0; //TEMP VARIABLE TO BE REPLACED WHEN LOOPS WORK

    public Image progressBarBit; //Progress bar first level image
    public Image progressBarHalf; //Progress bar second level image
    public Image progressBarFull; //Progress bar third level image


    //Function to keep track of the health bar removal
    public void healthBarStatus(float health)
    {
        healthText.text = "" + health; //Sets health to be displayed correctly on the HP bar
        float totalHealth = playerData.totalHealthBase; //sets a total health variable to the health base for fractioning
        float result = (int)health / totalHealth; //Sets the fraction for the scaling 
        healthBar.rectTransform.localScale = new Vector3 ((result * hpBarX),0.38f,0.38f); //Scales the hpBar image
        //Debug.Log(healthBar.rectTransform.localScale.x);
    }

    //Function to keep track of the Progress bar and what level the player is on
    public void progressStatus()
    {
        //Current level variable
        int currLevel = playerData.OnLevel; //0 is level 1, 1 is level 2, 2 is level 3
        //Debug.Log("We are on the " + currLevel+1 + " level!"); 

        //If-else statements to check what level we are currently on and what should be enabled or disabled
        //level 1 check
        if(currLevel == 0)
        {
            progressBarBit.enabled = true;
            progressBarHalf.enabled = false;
            progressBarFull.enabled = false;
        }

        //level 2 check
        if (currLevel == 1)
        {
            progressBarBit.enabled = false;
            progressBarHalf.enabled = true;
            progressBarFull.enabled = false;
        }

        //level 3 check
        if (currLevel == 2)
        {
            progressBarBit.enabled = false;
            progressBarHalf.enabled = false;
            progressBarFull.enabled = true;
        }

        //Debug.Log("Progress Bar Set!");
    }

    // Start is called before the first frame update
    void Start()
    {
        //When the scene starts it will display the current currency total that is stored in the player data
        currencyText.text = "" + playerData.currency;

        //When the scene starts it will display the current health total that is stored in the player data
        healthText.text = "" + playerData.localHealth;

        hpBarX = healthBar.rectTransform.localScale.x;

        //Set the text for loops completed *TEMP UNTIL LOOPS ARE ENABLED*
        loopsText.text = "" + loopsCompleted;

        //Initializes the progress bar
        progressStatus();
    }


    //Collision function for currency tracking
    private void OnTriggerEnter(Collider other)
    {
        //Checks to see if the other tag is Currency
        if(other.gameObject.tag == "Currency")
        {
            //Debug.Log("Got Currency!");
            playerData.currency += 1; //VARIABLE LOCATION TO CHANGE THE AMOUNT THAT CURRENCY IS WORTH *TEMP*
            Destroy(other.gameObject); //Destroys the currency obj
            currencyText.text = "" + playerData.currency; //Updates currency UI
        }

        //Checks if it was a bullet or enemy to adjust HP UI
        if(other.gameObject.tag == "Bullet" || other.gameObject.tag == "Enemy")
        {
            //Adjusts the text and image of the hp bar
            currHealth = currentPlayer.health;   //("NewPlayer").GetComponent<PlayerMovement>().health;
            healthBarStatus(currHealth);
        }
    }

    public void UpdateObjText()
    {
        objectiveTracker += 1;
        objectiveText.text = "-Find and press all the Yellow Buttons(" + objectiveTracker.ToString() + "/5).";
    }

}

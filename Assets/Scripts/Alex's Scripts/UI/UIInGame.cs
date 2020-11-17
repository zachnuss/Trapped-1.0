using System.Collections;
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
    public Text specialCurrencyText; //Added by Wesley for special currency, change this how you want later.
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

    //ADDED BY ZACHARY
    public GameObject vDamage;
    public GameObject vHealth;
    public GameObject vSpeed;

    //text for upgrades
    public Text speedUp;
    public Text healthUp;
    public Text damageUp;


    //Function to keep track of the health bar removal
    public void healthBarStatus(float health)
    {
        healthText.text = "" + (int)health; //Sets health to be displayed correctly on the HP bar
        float totalHealth = playerData.totalHealthBase; //sets a total health variable to the health base for fractioning
        float result = health / totalHealth; //Sets the fraction for the scaling 
        healthBar.rectTransform.localScale = new Vector3 ((result * hpBarX),1f,0.38f); //Scales the hpBar image
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

        //When the scene starts it will display the current special currency total that is stored in the player data - Added by Wesley
        specialCurrencyText.text = "" + playerData.specialCoins;

        //When the scene starts it will display the current health total that is stored in the player data
        healthText.text = "" + playerData.localHealth;

        hpBarX = healthBar.rectTransform.localScale.x;

        loopsCompleted = playerData.loops;
        //Set the text for loops completed *TEMP UNTIL LOOPS ARE ENABLED*
        loopsText.text = "" + loopsCompleted;

        //Initializes the progress bar
        progressStatus();

        updateUpgrades();

        

    }

    //ADDED BY ZACHARY
    public void updateUpgrades()
    {
        speedUp.text = playerData.speedUpgrade.ToString();
        healthUp.text = playerData.healthUpgrade.ToString();
        damageUp.text = playerData.damageUpgrade.ToString();
    }



    //Collision function for currency tracking
    private void OnTriggerEnter(Collider other)
    {
        //Checks to see if the other tag is Currency
        if(other.gameObject.tag == "Currency")
        {
            if (other.GetComponent<CurrencyType>().special == false) //added by Wesley, checks currency type.
            {
                //Debug.Log("Got Currency!");
                playerData.AddCurrency(1); //VARIABLE LOCATION TO CHANGE THE AMOUNT THAT CURRENCY IS WORTH *TEMP* //We have a function that does this + adds score and tracks data, updated it - Wesley
                Destroy(other.gameObject); //Destroys the currency obj
                currencyText.text = "" + playerData.currency; //Updates currency UI
            }
            if(other.GetComponent<CurrencyType>().special == true)
            {
                playerData.AddSpecialCoins(1);
                Destroy(other.gameObject);
                specialCurrencyText.text = "" + playerData.specialCoins; //Update special currency in the UI
            }

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
        objectiveText.text = "("+objectiveTracker.ToString() + "/5)";
    }

}

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
    
    //Color switching variables
    public GameObject MColor2Label;
    public GameObject MColor3Label;
    public GameObject FColor2Label;
    public GameObject FColor3Label;

    //Pet switching Variables
    public GameObject petButtons;
    public GameObject petNA;
    public GameObject petBee;
    public GameObject petBunny;
    
    //Pet purchase Variables
    public GameObject PurchaseP2; //Text box to display that the player needs to purchase player 2
    public GameObject PurchasePet; //Text box to display that the player needs to purchase pet

    //Weapon Switching Objects
    public GameObject Weapons; //The weapon models
    
    //Bools to keep track of what is active
    private bool playerActive = true; //Keeps track of player section being active
    private bool petActive = false; //Keeps track of pet section being active
    private bool weaponActive = false; //Keeps track of weapon section being active


    /// <summary>
    /// Alexander
    /// Updated: 12-7-2020
    /// 
    /// Function to switch between the character and the pets.
    /// </summary>
    public void switchToPets()
    {
        //Checks to see if the characters are active and then switch to the pets
        if(playerActive == true)
        {
            playerActive = false;
            petActive = true;
            players.SetActive(false);
            Prisoner.enabled = false;
            pets.SetActive(true);
            femaleColor.SetActive(false);
            maleColor.SetActive(false);
            petButtons.SetActive(true);
        }

        //Checks to see if the weapons are active then switches from them
        if (weaponActive == true)
        {
            weaponActive = false;
            petActive = true;
            Weapons.SetActive(false);
            Korben.enabled = false;
            pets.SetActive(true);
            petButtons.SetActive(true);
        }

        //Switches to the pet selection buttons
        EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
        EventSystem.current.SetSelectedGameObject(petNA); //Sets the selected obj
    }

    /// <summary>
    /// Alexander
    /// Updated: 12-7-2020
    /// 
    /// Function to switch between the character and the pets.
    /// </summary>
    public void switchCharacter()
    {
        //Checks to see if pets is active then turns it off
        if (petActive == true)
        {
            playerActive = true;
            petActive = false;
            pets.SetActive(false);
            Prisoner.enabled = true;
            players.SetActive(true);
            petButtons.SetActive(false);
        }

        //Checks to see if the weapons are active then switches from them
        if (weaponActive == true)
        {
            weaponActive = false;
            playerActive = true;
            Weapons.SetActive(false);
            Prisoner.enabled = true;
            Korben.enabled = false;
            players.SetActive(true);
        }

        if (playerData.characterModelSwitch == false) //False on the character model switch is Male, true is female
        {
            maleColor.SetActive(true);
        }
        else
            femaleColor.SetActive(true);
    }

    /// <summary>
    /// Alexander
    /// Updated: 11-6-2020
    /// 
    /// Function to switch to Weapons
    /// </summary>
    public void switchToWeapons()
    {
        //Checks to see if the characters are active and then switch to Weapons
        if (playerActive == true)
        {
            playerActive = false;
            weaponActive = true;
            players.SetActive(false);
            Prisoner.enabled = false;
            Korben.enabled = true;
            Weapons.SetActive(true);
            femaleColor.SetActive(false);
            maleColor.SetActive(false);
        }

        //Checks to see if pets is active then turns it off
        if (petActive == true)
        {
            weaponActive = true;
            petActive = false;
            Korben.enabled = true;
            pets.SetActive(false);
            petButtons.SetActive(false);
            Weapons.SetActive(true);
        }
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
            if (playerData.characterModelSwitch == false) //Checks to see if the Male model is up
            {
                //Uses the event system to set the buttons for the UI
                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(maleColor1); //Sets the selected obj
            }

            else //Checks to see if the Female model is up
            {
                //Uses the event system to set the buttons for the UI
                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(femaleColor1); //Sets the selected obj
            }
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

            if (playerData.character2Purchase == false) //The player doesn't have enough currency to switch between the models
            {
                StartCoroutine(waiting());
            }
        }

        //Checks to see if pets is active
        /*if (pets.activeInHierarchy == true)//If it is will adjust pet choice
        {
            playerData.petChoice++;//rb goes up, lb goes down
            if (playerData.petChoice >= 3) //loops if goes too far
                playerData.petChoice = 0;
            int petAttempt = playerData.petChoice;
            playerData.ChangePet(playerData.petChoice);//runs changepet
            if (petAttempt == 1)
            {
                if (playerData.characterPet2Purchase == false)//checks if pet is purchased after running changepet
                    StartCoroutine(waitingPet());
            }
            if (petAttempt == 2)
            {
                if (playerData.characterPet3Purchase == false)//checks if pet is purchased after running changepet
                    StartCoroutine(waitingPet());
            }
        }*/
    }

    public void OnLB()
    {
        //Checks to see if the prisoner is active
        if (playerActive == true) //If it is then it will go and adjust the players colors
        {
            playerData.SetCharacterChoiceMenu();

            if (playerData.character2Purchase == false) //The player doesn't have enough currency to switch between the models
            {
                StartCoroutine(waiting());
            }
        }

        //Checks to see if pets is active
        /*if (pets.activeInHierarchy == true)//If it is will adjust pet choice
        {
            playerData.petChoice--;//rb goes up, lb goes down
            if (playerData.petChoice <= -1)//loops if goes too far
                playerData.petChoice = 2;
            int petAttempt = playerData.petChoice;
            playerData.ChangePet(playerData.petChoice);//runs changepet
            if (petAttempt == 1)
            {
                if (playerData.characterPet2Purchase == false)//checks if pet is purchased after running changepet
                    StartCoroutine(waitingPet());
            }
            if (petAttempt == 2)
            {
                if (playerData.characterPet3Purchase == false)//checks if pet is purchased after running changepet
                    StartCoroutine(waitingPet());
            }
        }*/
    }

    /// <summary>
    /// Alexander
    /// Updated: 11-6-2020
    /// 
    /// Courutine to keep track of seconds waiting to display that players need to purchase the second model
    /// </summary>
    IEnumerator waiting()
    {
        PurchaseP2.SetActive(true);
        yield return new WaitForSeconds(2);
        PurchaseP2.SetActive(false);
    }

    /// <summary>
    /// Wesley
    /// Updated: 11-16-2020
    /// 
    /// Courutine to keep track of seconds waiting to display that players need to purchase a pet
    /// </summary>
    IEnumerator waitingPet()
    {
        PurchasePet.SetActive(true);
        yield return new WaitForSeconds(2);
        PurchasePet.SetActive(false);
    }

    /// <summary>
    /// Alexander
    /// Updated: 12-7-2020
    /// 
    /// Awake to making sure that the prisoner description is displaying and not the other images
    /// </summary>
    private void Awake()
    {
        Korben.enabled = false;
    }

        /// <summary>
        /// Alexander
        /// Updated: 11-6-2020
        /// 
        /// Updates to keep track of the color text box labels to see which ones should be active and which shouldn't
        /// </summary>
        private void Update()
    {
        //Checks to see if the prisoner is active
        if (playerActive == true) //If it is then it will go and adjust the players colors
        {
            if (playerData.characterModelSwitch == false) //False on the character model switch is Male, true is female
            {
                femaleColor.SetActive(false);
                maleColor.SetActive(true);

                //All the if else statments to check for the colors
                if (playerData.character1Color3 == false)
                {
                    MColor2Label.SetActive(true);
                }
                else
                {
                    MColor2Label.SetActive(false);
                }

                if (playerData.character1Color2 == false)
                {
                    MColor3Label.SetActive(true);
                }
                else
                {
                    MColor3Label.SetActive(false);
                }
            }

            else //False on the character model switch is Male, true is female
            {
                femaleColor.SetActive(true);
                maleColor.SetActive(false);

                //All the if else statments to check for the colors
                if (playerData.character2Color2 == false)
                {
                    FColor3Label.SetActive(true);
                }
                else
                {
                    FColor3Label.SetActive(false);
                }

                if (playerData.character2Color3 == false)
                {
                    FColor2Label.SetActive(true);
                }
                else
                {
                    FColor2Label.SetActive(false);
                }
            }
        }
    }
}

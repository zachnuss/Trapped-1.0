﻿using System.Collections;
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
    public GameObject weapon1Color;
    public GameObject weapon2Color;
    public GameObject weapon3Color;
    public GameObject weapon1Color1;
    public GameObject weapon2Color1;
    public GameObject weapon3Color1;

    //Color switching variables
    public GameObject MColor2Label;
    public GameObject MColor3Label;
    public GameObject FColor2Label;
    public GameObject FColor3Label;
    public GameObject W1Color2Label;
    public GameObject W1Color3Label;
    public GameObject W2Color2Label;
    public GameObject W2Color3Label;
    public GameObject W3Color2Label;
    public GameObject W3Color3Label;

    //Pet switching Variables
    public GameObject petButtons;
    public GameObject petNA;
    public GameObject petBee;
    public GameObject petBunny;
    public GameObject BeeDescription;
    public GameObject BunnyDescription;
    public GameObject BeeModel;
    public GameObject BunnyModel;
    private int currPet;
    public GameObject BeeLabel;
    public GameObject BunnyLabel;

    //Purchase System, yes it's jank
    public GameObject purchaseConfirmation; //textbox for purchases
    private bool purchaseYes;
    private bool purchaseNo;
    
    //Purchase Label Objects
    public GameObject PurchaseP2; //Text box to display that the player needs to purchase player 2
    public GameObject PurchaseWeapon;
    public GameObject PurchasePet; //Text box to display that the player needs to purchase pet

    //Weapon Switching Objects
    public GameObject Weapons; //The weapon models

    //Button Objects
    public GameObject Character; //Character Button
    public GameObject WeaponSelect; //Weapon Button
    public GameObject PurchaseYes; //Yes to buy character 2
    public GameObject PurchaseNo; //No to buy character 2

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
            weapon1Color.SetActive(false);
            weapon2Color.SetActive(false);
            weapon3Color.SetActive(false);
            petButtons.SetActive(true);
        }

        //Checks to see if the weapons are active then switches from them
        if (weaponActive == true)
        {
            weaponActive = false;
            petActive = true;
            Weapons.SetActive(false);
            Korben.enabled = false;
            Merlon.enabled = false;
            Lazurus.enabled = false;
            pets.SetActive(true);
            petButtons.SetActive(true);
        }

        if (playerData.characterPet2Purchase == true)//checks if pet is purchased after running changepet
            BeeLabel.SetActive(false);
        if (playerData.characterPet3Purchase == true)//checks if pet is purchased after running changepet
            BunnyLabel.SetActive(false);

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
            BeeDescription.SetActive(false);
            BunnyDescription.SetActive(false);
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
            Korben.enabled = false;
            Prisoner.enabled = true;
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
            Merlon.enabled = true;
            Lazurus.enabled = true;
            Weapons.SetActive(true);
            femaleColor.SetActive(false);
            maleColor.SetActive(false);
        }

        //Checks to see if pets is active then turns it off
        if (petActive == true)
        {
            weaponActive = true;
            petActive = false;
            pets.SetActive(false);
            BeeDescription.SetActive(false);
            BunnyDescription.SetActive(false);
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
        if(weaponActive == true)
        {
            switch (playerData.weaponModelChoice)
            {
                case 0:
                    weapon1Color.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                    EventSystem.current.SetSelectedGameObject(weapon1Color1); //Sets the selected obj
                    break;
                case 1:
                    weapon2Color.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                    EventSystem.current.SetSelectedGameObject(weapon2Color1); //Sets the selected obj
                    break;
                case 2:
                    weapon3Color.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                    EventSystem.current.SetSelectedGameObject(weapon3Color1); //Sets the selected obj
                    break;
                default:
                    Debug.Log("something went wrong at line 257 in charcustom script");
                    break;
            }
        }
    }

    /// <summary>
    /// Alexander
    /// Updated: 12-7-2020
    /// 
    /// Pet buttons to track the pet selection and all that
    /// </summary>
    public void NAButton()
    {
        if (currPet == 1) //From Bee
        {
            BeeModel.GetComponent<MeshRenderer>().enabled = false;
            BeeDescription.SetActive(false);
            playerData.ChangePet(0);//runs changepet
            currPet = 0;
        }

        if (currPet == 2) //From Bunny
        {
            BunnyModel.GetComponent<MeshRenderer>().enabled = false;
            BunnyDescription.SetActive(false);
            playerData.ChangePet(0);//runs changepet
            currPet = 0;
        }
    }

    public void BeeButton()
    {
        if (currPet == 0) //From NA
        {
            BeeModel.GetComponent<MeshRenderer>().enabled = true;
            BeeDescription.SetActive(true);
            currPet = 1;

            if (playerData.characterPet2Purchase == false)//checks if pet is purchased after running changepet
                StartCoroutine(waitingPet());
            if (playerData.characterPet2Purchase == true)//checks if pet is purchased after running changepet
                playerData.ChangePet(1);//runs changepet
        }

        if (currPet == 2) //From Bunny
        {
            BunnyModel.GetComponent<MeshRenderer>().enabled = false;
            BeeModel.GetComponent<MeshRenderer>().enabled = true;
            BunnyDescription.SetActive(false);
            BeeDescription.SetActive(true);
            currPet = 1;

            if (playerData.characterPet2Purchase == false)//checks if pet is purchased after running changepet
                StartCoroutine(waitingPet());
            if (playerData.characterPet2Purchase == true)//checks if pet is purchased after running changepet
                playerData.ChangePet(1);//runs changepet
        }
    }

    public void BunnyButton()
    {
        if(currPet == 0) //From NA
        {
            BunnyModel.GetComponent<MeshRenderer>().enabled = true;
            BunnyDescription.SetActive(true);
            currPet = 2;

            if (playerData.characterPet3Purchase == false)//checks if pet is purchased after running changepet
                StartCoroutine(waitingPet());
            if (playerData.characterPet3Purchase == true)//checks if pet is purchased after running changepet
                playerData.ChangePet(2);//runs changepet
        }   

        if (currPet == 1) //From Bee
        {
            BeeModel.GetComponent<MeshRenderer>().enabled = false;
            BunnyModel.GetComponent<MeshRenderer>().enabled = true;
            BeeDescription.SetActive(false);
            BunnyDescription.SetActive(true);
            currPet = 2;

            if (playerData.characterPet3Purchase == false)//checks if pet is purchased after running changepet
                StartCoroutine(waitingPet());
            if (playerData.characterPet3Purchase == true)//checks if pet is purchased after running changepet
                playerData.ChangePet(2);//runs changepet
        }
    }

    /// <summary>
    /// Alexander
    /// Updated: 12-7-2020
    /// 
    /// Two Functions that have the same purpose
    /// Switches models based on right or left bumper being pressed
    /// </summary>
    public void OnRB()
    {
        //Checks to see if the prisoner is active
        if (playerActive == true) //If it is then it will go and adjust the players colors
        {
            if(playerData.character2Purchase == false && playerData.specialCoins > playerData.characterCost)
            {
                purchaseConfirmation.SetActive(true);
                
                //Sets second character to on
                GameObject[] character1 = new GameObject[8];
                // GameObject[] character2 = new GameObject[8];
                GameObject character2 = GameObject.Find("secondCharacter_low");
                //GameObject character1 = GameObject.Find("MainCharacter_Geo");
                for (int i = 0; i < character1.Length; i++)
                {
                    character1[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
                }
                character2.GetComponent<SkinnedMeshRenderer>().enabled = true;
                for (int i = 0; i < character1.Length; i++)
                {
                    character1[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                if (GameObject.Find("secondCharacter_low") == true)
                {
                    //Debug.Log("Second step");
                    GameObject character;
                    character = GameObject.Find("secondCharacter_low").gameObject;
                    character.GetComponent<SkinnedMeshRenderer>().material = playerData.player2Color[playerData.materialChoice2];
                }

                //Uses the event system to set the buttons for the UI
                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(PurchaseYes); //Sets the selected obj
            }

            else if (playerData.character2Purchase == false) //The player doesn't have enough currency to switch between the models
            {
                StartCoroutine(waiting());
            }

            else if (playerData.character2Purchase == true)
            {
                playerData.SetCharacterChoiceMenu();
                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(Character); //Sets the selected obj
            }
        }

        //If weapon is active
        if (weaponActive)
        {
            playerData.weaponModelChoice++;
            if (playerData.weaponModelChoice > 2)
                playerData.weaponModelChoice = 0;
            GameObject gun1 = GameObject.Find("Gun_V2");
            GameObject gun2 = GameObject.Find("Gun2_V2");
            GameObject gun3 = GameObject.Find("Gun3_V2");
            switch (playerData.weaponModelChoice)
            {
                case 0:
                    playerData.SetWeaponChoiceMenu();
                    gun2.GetComponent<MeshRenderer>().enabled = false;
                    gun3.GetComponent<MeshRenderer>().enabled = false;

                    //uses the event system to set the buttons for the ui
                    purchaseConfirmation.SetActive(false); //turns off purchase confirmation
                    EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                    EventSystem.current.SetSelectedGameObject(WeaponSelect); //Sets the selected obj
                    break;
                case 1:
                    if (playerData.weapon2Purchase == false && playerData.specialCoins >= playerData.characterCost)
                    {
                        purchaseConfirmation.SetActive(true);

                        //Sets second gun to on
                        gun2.GetComponent<MeshRenderer>().enabled = true;
                        gun1.GetComponent<MeshRenderer>().enabled = false;
                        gun3.GetComponent<MeshRenderer>().enabled = false;

                        //Uses the event system to set the buttons for the UI
                        EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                        EventSystem.current.SetSelectedGameObject(PurchaseYes); //Sets the selected obj
                    }

                    else if (playerData.weapon2Purchase == false) //The player doesn't have enough currency to switch between the models
                    {
                        StartCoroutine(waitingWeapon());
                    }

                    else if (playerData.weapon2Purchase == true) //if the model we moved to is purchased
                    {
                        playerData.SetWeaponChoiceMenu();
                        purchaseConfirmation.SetActive(false); //turns off purchase confirmation
                        EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                        EventSystem.current.SetSelectedGameObject(WeaponSelect); //Sets the selected obj
                    }
                    break;
                case 2:
                    if (playerData.weapon3Purchase == false && playerData.specialCoins >= playerData.characterCost)
                    {
                        purchaseConfirmation.SetActive(true);

                        //Sets second gun to on
                        gun3.GetComponent<MeshRenderer>().enabled = true;
                        gun1.GetComponent<MeshRenderer>().enabled = false;
                        gun2.GetComponent<MeshRenderer>().enabled = false;

                        //Uses the event system to set the buttons for the UI
                        EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                        EventSystem.current.SetSelectedGameObject(PurchaseYes); //Sets the selected obj
                    }

                    else if (playerData.weapon3Purchase == false) //The player doesn't have enough currency to switch between the models
                    {
                        StartCoroutine(waitingWeapon());
                    }

                    else if (playerData.weapon3Purchase == true) //if the model we moved to is purchased
                    {
                        playerData.SetWeaponChoiceMenu();
                        purchaseConfirmation.SetActive(false); //turns off purchase confirmation
                        EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                        EventSystem.current.SetSelectedGameObject(WeaponSelect); //Sets the selected obj
                    }
                    break;
                default:
                    Debug.Log("Something went wrong in line 399");
                    break;
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
            if (playerData.character2Purchase == false && playerData.specialCoins > playerData.characterCost)
            {
                purchaseConfirmation.SetActive(true);

                //Sets second character to on
                GameObject[] character1 = new GameObject[8];
                // GameObject[] character2 = new GameObject[8];
                GameObject character2 = GameObject.Find("secondCharacter_low");
                //GameObject character1 = GameObject.Find("MainCharacter_Geo");
                for (int i = 0; i < character1.Length; i++)
                {
                    character1[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
                }
                character2.GetComponent<SkinnedMeshRenderer>().enabled = true;
                for (int i = 0; i < character1.Length; i++)
                {
                    character1[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                if (GameObject.Find("secondCharacter_low") == true)
                {
                    //Debug.Log("Second step");
                    GameObject character;
                    character = GameObject.Find("secondCharacter_low").gameObject;
                    character.GetComponent<SkinnedMeshRenderer>().material = playerData.player2Color[playerData.materialChoice2];
                }

                //Uses the event system to set the buttons for the UI
                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(PurchaseYes); //Sets the selected obj


            }

            else if (playerData.character2Purchase == false) //The player doesn't have enough currency to switch between the models
            {
                StartCoroutine(waiting());
            }

            else if (playerData.character2Purchase == true)
            {
                playerData.SetCharacterChoiceMenu();
                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(Character); //Sets the selected obj
            }
        }

        if (weaponActive)
        {
            playerData.weaponModelChoice--;
            if (playerData.weaponModelChoice < 0)
                playerData.weaponModelChoice = 2;
            GameObject gun1 = GameObject.Find("Gun_V2");
            GameObject gun2 = GameObject.Find("Gun2_V2");
            GameObject gun3 = GameObject.Find("Gun3_V2");
            switch (playerData.weaponModelChoice)
            {
                case 0:
                    playerData.SetWeaponChoiceMenu();
                    gun2.GetComponent<MeshRenderer>().enabled = false;
                    gun3.GetComponent<MeshRenderer>().enabled = false;
                    purchaseConfirmation.SetActive(false); //turns off purchase confirmation
                    EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                    EventSystem.current.SetSelectedGameObject(WeaponSelect); //Sets the selected obj
                    break;
                case 1:
                    if (playerData.weapon2Purchase == false && playerData.specialCoins >= playerData.characterCost)
                    {
                        purchaseConfirmation.SetActive(true);

                        //turns other guns on or off
                        gun2.GetComponent<MeshRenderer>().enabled = true;
                        gun1.GetComponent<MeshRenderer>().enabled = false;
                        gun3.GetComponent<MeshRenderer>().enabled = false;

                        //Uses the event system to set the buttons for the UI
                        EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                        EventSystem.current.SetSelectedGameObject(PurchaseYes); //Sets the selected obj
                    }

                    else if (playerData.weapon2Purchase == false) //The player doesn't have enough currency to switch between the models
                    {
                        StartCoroutine(waitingWeapon());
                    }

                    else if (playerData.weapon2Purchase == true) //if the model we moved to is purchased
                    {
                        playerData.SetWeaponChoiceMenu();
                        purchaseConfirmation.SetActive(false); //turns off purchase confirmation
                        EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                        EventSystem.current.SetSelectedGameObject(WeaponSelect); //Sets the selected obj
                    }
                    break;
                case 2:
                    if (playerData.weapon3Purchase == false && playerData.specialCoins >= playerData.characterCost)
                    {
                        purchaseConfirmation.SetActive(true);

                        //sets guns on or off for preview
                        gun3.GetComponent<MeshRenderer>().enabled = true;
                        gun2.GetComponent<MeshRenderer>().enabled = false;
                        gun1.GetComponent<MeshRenderer>().enabled = false;

                        //Uses the event system to set the buttons for the UI
                        EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                        EventSystem.current.SetSelectedGameObject(PurchaseYes); //Sets the selected obj
                    }

                    else if (playerData.weapon3Purchase == false) //The player doesn't have enough currency to switch between the models
                    {
                        StartCoroutine(waitingWeapon());
                    }

                    else if (playerData.weapon3Purchase == true) //if the model we moved to is purchased
                    {
                        playerData.SetWeaponChoiceMenu();
                        purchaseConfirmation.SetActive(false); //turns off purchase confirmation
                        EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                        EventSystem.current.SetSelectedGameObject(WeaponSelect); //Sets the selected obj
                    }
                    break;
                default:
                    Debug.Log("Something went wrong in line 549");
                    break;
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
    /// Updated: 12-7-2020
    /// 
    /// Courutine to keep track of seconds waiting to display that players need to purchase the second model
    /// </summary>
    IEnumerator waitingWeapon()
    {
        PurchaseWeapon.SetActive(true);
        yield return new WaitForSeconds(2);
        PurchaseWeapon.SetActive(false);
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
    

    public void ConfirmPurchase()
    {
        purchaseYes = true;
    }

    public void CancelPurchase()
    {
        purchaseNo = true;
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
        Merlon.enabled = false;
        Lazurus.enabled = false;
        currPet = playerData.petChoice;
        if (playerData.achievementRunner == true)
            playerData.characterPet3Purchase = true;
        if (playerData.achievementRevenge1 == true)
            playerData.characterPet2Purchase = true;
        if (playerData.achievementRunner == false)
            playerData.characterPet3Purchase = false;
        if (playerData.achievementRevenge1 == false)
            playerData.characterPet2Purchase = false;
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
            
            //PurchaseControl
            if (purchaseYes == true)
            {
                purchaseConfirmation.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(Character); //Sets the selected obj
                playerData.SetCharacterChoiceMenu();
                purchaseYes = false;
            }
            if (purchaseNo == true)
            {
                purchaseConfirmation.SetActive(false);
                GameObject[] character1 = new GameObject[8];
                // GameObject[] character2 = new GameObject[8];
                GameObject character2 = GameObject.Find("secondCharacter_low");

                //GameObject character1 = GameObject.Find("MainCharacter_Geo");
                for (int i = 0; i < character1.Length; i++)
                {
                    character1[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
                }
                character2.GetComponent<SkinnedMeshRenderer>().enabled = false;
                for (int i = 0; i < character1.Length; i++)
                {
                    character1[i].GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(Character); //Sets the selected obj
                purchaseNo = false;
            }
        }

        //Weapons active
        if(weaponActive == true)
        {
            if(playerData.weaponModelChoice == 0)
            {
                Korben.GetComponent<Image>().enabled = true;
                Merlon.GetComponent<Image>().enabled = false;
                Lazurus.GetComponent<Image>().enabled = false;
            }

            if (playerData.weaponModelChoice == 1)
            {
                Korben.GetComponent<Image>().enabled = false;
                Lazurus.GetComponent<Image>().enabled = true;
                Merlon.GetComponent<Image>().enabled = false;
            }

            if (playerData.weaponModelChoice == 2)
            {
                Korben.GetComponent<Image>().enabled = false;
                Lazurus.GetComponent<Image>().enabled = false;
                Merlon.GetComponent<Image>().enabled = true;
            }


            switch (playerData.weaponModelChoice)
            {
                case 0:
                    weapon1Color.SetActive(true);
                    weapon2Color.SetActive(false);
                    weapon3Color.SetActive(false);
                    //All the if else statments to check for the colors
                    if (playerData.weapon1Color2 == false)
                        W1Color2Label.SetActive(true);
                    else
                        W1Color2Label.SetActive(false);

                    if (playerData.weapon1Color3 == false)
                        W1Color3Label.SetActive(true);
                    else
                        W1Color3Label.SetActive(false);
                    break;
                case 1:
                    weapon2Color.SetActive(true);
                    weapon1Color.SetActive(false);
                    weapon3Color.SetActive(false);
                    //All the if else statments to check for the colors
                    if (playerData.weapon2Color2 == false)
                        W2Color2Label.SetActive(true);
                    else
                        W2Color2Label.SetActive(false);

                    if (playerData.weapon2Color3 == false)
                        W2Color3Label.SetActive(true);
                    else
                        W2Color3Label.SetActive(false);
                    break;
                case 2:
                    weapon3Color.SetActive(true);
                    weapon1Color.SetActive(false);
                    weapon2Color.SetActive(false);
                    //All the if else statments to check for the colors
                    if (playerData.weapon3Color2 == false)
                        W3Color2Label.SetActive(true);
                    else
                        W3Color2Label.SetActive(false);

                    if (playerData.weapon3Color3 == false)
                        W3Color3Label.SetActive(true);
                    else
                        W3Color3Label.SetActive(false);
                    break;
                default:
                    Debug.Log("something went wrong at line 791 in charcustom script");
                    break;
            }
            
            //PurchaseControl
            if (purchaseYes == true)
            {
                purchaseConfirmation.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(WeaponSelect); //Sets the selected obj
                playerData.SetWeaponChoiceMenu();
                purchaseYes = false;
            }
            if (purchaseNo == true)
            {
                purchaseConfirmation.SetActive(false);

                GameObject gun1 = GameObject.Find("Gun_V2");
                GameObject gun2 = GameObject.Find("Gun2_V2");
                GameObject gun3 = GameObject.Find("Gun3_V2");

                playerData.weaponModelChoice = 0;
                playerData.SetWeaponChoiceMenu();

                EventSystem.current.SetSelectedGameObject(null); //Clears the selected Obj
                EventSystem.current.SetSelectedGameObject(WeaponSelect); //Sets the selected obj
                purchaseNo = false;
            }
        }

        
    }
}

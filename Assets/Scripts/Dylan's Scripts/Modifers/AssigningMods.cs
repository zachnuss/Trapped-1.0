using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;


/// <summary>
/// After a complete run (lvls 1 - 3), player will be able to select which mods to activate for next run
/// </summary>
public class AssigningMods : MonoBehaviour
{
    public PlayerData playerData;
    public GameLevelData gameLevelData;

    public Button[] buttonArray;
    public Text[] appliedText;
    public Text modsText;
    public Text modsActiveText;

    public int numberOfModsToSelect;
   // this ui is only temp 
    public ColorBlock disabledColor;

    public modifierType type1;
    public modifierType type2;

    public Text mod1Descript1;
    public Text mod2Descript2;

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-15-2020
    /// 
    /// Sets colors on buttons and establishes how many mods it can assign. Skips this if there are no mods to select
    /// </summary>
    private void Start()
    {
        
        numberOfModsToSelect = Mathf.FloorToInt(playerData.levelsBeaten/3);
        
        //Debug.Log(numberOfModsToSelect);
        //_myEventSystem = GameObject.Find("EventSystem");
        disabledColor.highlightedColor = buttonArray[0].colors.highlightedColor;
        disabledColor.pressedColor = buttonArray[0].colors.pressedColor;
        disabledColor.normalColor = buttonArray[0].colors.disabledColor;
        disabledColor.selectedColor = buttonArray[0].colors.selectedColor;
        disabledColor.selectedColor = new Color(200, 200, 200);

        ButtonsActiveInitial();

        gameLevelData.UpdateModCounter();
        ChooseRandomButton();
        ToStore();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets texts function every frame
    /// </summary>
    private void Update()
    {
        SetText();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// buttons are unclickable if they have already been applied
    /// </summary>
    void ButtonsActiveInitial()
    {
        // Debug.Log("yes");
        for (int modIndex = 0; modIndex < gameLevelData.mods.Length; modIndex++)
        {
            if(gameLevelData.mods[modIndex].modActive)
            {
               // buttonArray[buttonIndex].interactable = false;
               // buttonArray[modIndex].colors = disabledColor;
               // Debug.Log(appliedText[buttonIndex].name);
                //appliedText[modIndex].text = "APPLIED";
                numberOfModsToSelect--;
            }
        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// sets buttons that already have the mods on
    /// </summary>
    void ButtonsActiveCheck()
    {
        for (int modIndex = 0; modIndex < gameLevelData.mods.Length; modIndex++)
        {
            if (gameLevelData.mods[modIndex].modType == type1 && gameLevelData.mods[modIndex].modActive)
            {
                // buttonArray[buttonIndex].interactable = false;
                buttonArray[0].colors = disabledColor;
                //appliedText[buttonIndex].enabled = false;
                appliedText[0].text = "APPLIED";
                //Debug.Log("here");
                //numberOfModsToSelect--;
            }
            if (gameLevelData.mods[modIndex].modType == type2 && gameLevelData.mods[modIndex].modActive)
            {
                buttonArray[1].colors = disabledColor;
                appliedText[1].text = "APPLIED";
            }

        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-15-2020
    /// 
    /// looks up specific type we are turning on
    /// </summary>
    public void AssignMod(int buttonNum)
    {

        modifierType typeL;
        if (buttonNum == 0)
        {
            typeL = type1;
        }
        else
            typeL = type2;

        if (typeL != modifierType.none)
        {
            for (int modIndex = 0; modIndex < gameLevelData.mods.Length; modIndex++)
            {
                if (gameLevelData.mods[modIndex].modType == typeL)
                {
                    gameLevelData.mods[modIndex].modActive = true;
                    // gameLevelData.mods[modNum - 1].modActive = true;
                    numberOfModsToSelect--;
                    ButtonsActiveCheck();
                    gameLevelData.UpdateModCounter();
                    //         SceneManager.LoadScene("StoreScene");
                }
            }
        }

    }

    /// <summary>
    /// Dylan Loe
    /// Updated 11-15-2020
    /// 
    /// 
    /// </summary>
    void ChooseRandomButton()
    {
        if (gameLevelData.totalModsOn < 6)
        {
            int randomMod1 = Random.Range(0, gameLevelData.mods.Length);
            while (gameLevelData.mods[randomMod1].modActive == true)
            {
                randomMod1 = Random.Range(0, gameLevelData.mods.Length);
            }
            type1 = gameLevelData.mods[randomMod1].modType;
            //set text
            string description = gameLevelData.mods[randomMod1].modDescription;
            //description.Replace("~", "\n");
            mod1Descript1.text = description.Replace("~", "\n");
            //mod1Descript1.text.Replace("~", "\n");

            Debug.Log("Modifier 1 = " + type1.ToString());
        }
        else
        {
            buttonArray[0].colors = disabledColor;
            //appliedText[buttonIndex].enabled = false;
            appliedText[0].text = "APPLIED";
        }


        if (gameLevelData.totalModsOn < 5)
        {
            int randomMod2 = Random.Range(0, gameLevelData.mods.Length);
            while (gameLevelData.mods[randomMod2].modActive == true || gameLevelData.mods[randomMod2].modType == type1)
            {
                randomMod2 = Random.Range(0, gameLevelData.mods.Length);
            }
            type2 = gameLevelData.mods[randomMod2].modType;
            //set text
            string description2 = gameLevelData.mods[randomMod2].modDescription;
            //description2.Replace("~", "\n");
            mod2Descript2.text = description2.Replace("~", "\n");
            //mod2Descript2.text.Replace("~", "\n");

            // mod2Descript2.text = gameLevelData.mods[randomMod2].modDescription;
            Debug.Log("Modifier 2 = " + type2.ToString());
        }
        else
        {
            mod2Descript2.text = "No More Mods Avalible...";
            buttonArray[1].colors = disabledColor;
            //appliedText[buttonIndex].enabled = false;
            appliedText[1].text = "APPLIED";
        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// wont go to store scene until all the mods are selected
    /// </summary>
    public void ToStore()
    {
        if(numberOfModsToSelect <= 0)
            SceneManager.LoadScene("StoreScene");
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-15-2020
    /// 
    /// set text on which mods are avalible
    /// </summary>
    public void SetText()
    {
        modsText.text = "Mods Avalible: " + numberOfModsToSelect.ToString();
        modsActiveText.text = "Mods Active: " + gameLevelData.totalModsOn.ToString();
    }
}

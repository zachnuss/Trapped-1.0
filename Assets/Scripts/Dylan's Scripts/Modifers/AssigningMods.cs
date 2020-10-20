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

    public int numberOfModsToSelect;
   // this ui is only temp 
    public ColorBlock disabledColor;

    private void Start()
    {
        ToStore();
        numberOfModsToSelect = Mathf.FloorToInt(playerData.levelsBeaten/3);
        Debug.Log(numberOfModsToSelect);
        //_myEventSystem = GameObject.Find("EventSystem");
        disabledColor.highlightedColor = buttonArray[0].colors.highlightedColor;
        disabledColor.pressedColor = buttonArray[0].colors.pressedColor;
        disabledColor.normalColor = buttonArray[0].colors.disabledColor;
        disabledColor.selectedColor = buttonArray[0].colors.selectedColor;
        disabledColor.selectedColor = new Color(200, 200, 200);
        ButtonsActiveInitial();
    }

    private void Update()
    {
        SetText();
    }

    //buttons are unclickable if they have already been applied
    void ButtonsActiveInitial()
    {
       // Debug.Log("yes");
        for(int buttonIndex = 0; buttonIndex <= buttonArray.Length - 1; buttonIndex++)
        {
            if(gameLevelData.mods[buttonIndex].modActive)
            {
               // buttonArray[buttonIndex].interactable = false;
                buttonArray[buttonIndex].colors = disabledColor;
                Debug.Log(appliedText[buttonIndex].name);
                appliedText[buttonIndex].text = "APPLIED";
                numberOfModsToSelect--;
            }
        }
    }

    void ButtonsActiveCheck()
    {
        for (int buttonIndex = 0; buttonIndex <= buttonArray.Length - 1; buttonIndex++)
        {
            if (gameLevelData.mods[buttonIndex].modActive)
            {
                // buttonArray[buttonIndex].interactable = false;
                buttonArray[buttonIndex].colors = disabledColor;
                //appliedText[buttonIndex].enabled = false;
                appliedText[buttonIndex].text = "APPLIED";
                Debug.Log("here");
                //numberOfModsToSelect--;
            }
        }
    }

    //looks up specific type we are turning on
    public void AssignMod(modifierType type)
    {
        for (int modIndex = 0; modIndex < gameLevelData.mods.Length; modIndex++)
        {
            if (gameLevelData.mods[modIndex].modType == type)
            {
                gameLevelData.mods[modIndex].modActive = true;
               // Debug.Log("yes");
            }
        }
    }


    //Assumes modNum is the number of mod not the index on array
    public void AssignMod(int modNum)
    {
        if(gameLevelData.mods[modNum - 1].modActive != true)
        {
            Debug.Log("Assigned Mod number " + modNum);
            gameLevelData.mods[modNum - 1].modActive = true;
            numberOfModsToSelect--;
            ButtonsActiveCheck();
        }

    }

    public void ToStore()
    {
        //wont go until we have mods selected
        if(numberOfModsToSelect <= 0)
            SceneManager.LoadScene("StoreScene");
    }

    public void SetText()
    {
        modsText.text = "Mods Avalible: " + numberOfModsToSelect.ToString();
    }
}

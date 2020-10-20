//Sets up character customization scene on load
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCharCustomizationLoad : MonoBehaviour
{
    public PlayerData playerData;
    public UnityEngine.UI.Toggle choiceToggle;
    void Start()
    {
        playerData.SetCharacterChoiceMenu(choiceToggle);
        playerData.SetMenuColor(playerData.materialChoice);
    }
}

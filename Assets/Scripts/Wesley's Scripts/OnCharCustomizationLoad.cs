//Sets up character customization scene on load
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCharCustomizationLoad : MonoBehaviour
{
    public PlayerData playerData;
    void Start()
    {
        playerData.SetCharacterChoiceGame();
        playerData.SetColor();
        playerData.SetPet();
    }
}

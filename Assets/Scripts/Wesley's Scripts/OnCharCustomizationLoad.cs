//Sets up character customization scene on load
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCharCustomizationLoad : MonoBehaviour
{
    public PlayerData playerData;

    //public GameObject playerRig1;
    //public GameObject playerRig2;


    void Start()
    {
        playerData.SetCharacterChoiceGame();
        playerData.SetColor();
    }
}

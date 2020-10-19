using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCharCustomizationLoad : MonoBehaviour
{
    public PlayerData playerData;
    void Start()
    {
        playerData.SetMenuColor(playerData.materialChoice);
    }
}

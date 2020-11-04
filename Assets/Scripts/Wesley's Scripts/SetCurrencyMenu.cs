using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCurrencyMenu : MonoBehaviour
{
    public PlayerData playerData;
    public Text specialCurrencyText;
    void Update()
    {
        specialCurrencyText.text = "" + playerData.specialCoins;
    }
}

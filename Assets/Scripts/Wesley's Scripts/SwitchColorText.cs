using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchColorText : MonoBehaviour
{
    public string boolFalse;
    public string boolTrue;
    public Text targetText;
    public PlayerData playerdata;
    public bool color2;

    private void Update()
    {
        if (color2 == true)
        {
            SwitchTextColor2();
        }
        else
        {
            SwitchTextColor3();
        }
    }

    public void SwitchTextColor2()
    {
        if (playerdata.characterModelSwitch == false)
        {
            if (playerdata.character1Color2 == false)
            {
                targetText.text = boolFalse;
            }
            else
            {
                targetText.text = boolTrue;
            }
        }
        else
        {
            if (playerdata.character2Color2 == false)
            {
                targetText.text = boolFalse;
            }
            else
            {
                targetText.text = boolTrue;
            }
        }
    }
    public void SwitchTextColor3()
    {
        if (playerdata.characterModelSwitch == false)
        {
            if (playerdata.character1Color3 == false)
            {
                targetText.text = boolFalse;
            }
            else
            {
                targetText.text = boolTrue;
            }
        }
        else
        {
            if (playerdata.character2Color3 == false)
            {
                targetText.text = boolFalse;
            }
            else
            {
                targetText.text = boolTrue;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggleText : MonoBehaviour
{
    public string boolFalse;
    public string boolTrue;
    public Text targetText;
    public PlayerData playerdata;

    private void Update()
    {
        SwitchText();
    }

    public void SwitchText()
    {
        if (playerdata.character2Purchase == false)
        {
            targetText.text = boolFalse;
        }
        else
        {
            targetText.text = boolTrue;
        }
    }
}

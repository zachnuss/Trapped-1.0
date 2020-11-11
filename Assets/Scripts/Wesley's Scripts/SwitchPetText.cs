using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPetText : MonoBehaviour
{
    public string boolFalse;
    public string boolTrue;
    public Text targetText;
    public PlayerData playerdata;
    public bool pet2;

    private void Update()
    {
        if (pet2 == true)
        {
            //SwitchTextPet2();
        }
        else
        {
            //SwitchTextPet3();
        }
    }

    /*public void SwitchTextPet2()
    {
        if (playerdata.characterPet2Purchase == false)
        {
            targetText.text = boolFalse;
        }
        else
        {
            targetText.text = boolTrue;
        }
    }

    public void SwitchTextPet3()
    {
        if (playerdata.characterPet3Purchase == false)
        {
            targetText.text = boolFalse;
        }
        else
        {
            targetText.text = boolTrue;
        }
    }*/

}

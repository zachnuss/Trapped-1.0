using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawning : MonoBehaviour
{
    //when level starts we choose one of the spots place in the level to be the button, the other spots

    public GameObject[] possibleButtons;
    public int indexChoosen;

    /// <summary>
    /// Dylan Loe
    /// 10-26-2020
    /// 
    /// turns on button for one of the preplaced objs in level
    /// </summary>
    public void AssignButton()
    {
        indexChoosen = Random.Range(0, possibleButtons.Length - 1);
        
        for(int objs = 0; objs <= possibleButtons.Length - 1; objs++)
        {
            if(objs == indexChoosen)
            {
                possibleButtons[indexChoosen].GetComponent<SetButtonOnObj>().SetUpObj(true);
            }
            else
                possibleButtons[indexChoosen].GetComponent<SetButtonOnObj>().SetUpObj(false);
        }
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawning : MonoBehaviour
{
    /// <summary>
    /// CURRENT SYSTEM CHOOSES ONE PER ButtonSpawning obj in scene, meaning there will need to be 1 ButtonSpawning Obj per side of cube (1 button per side)
    /// </summary>


    //when level starts we choose one of the spots place in the level to be the button, the other spots
    [Header("All the possible button children go here")]
    public GameObject[] possibleButtons;
    [Header("The choosen button to spawn goes here")]
    public int indexChoosen;

    private void Start()
    {
        AssignButton();
    }

    /// <summary>
    /// Dylan Loe
    /// 10-26-2020
    /// 
    /// turns on button for one of the preplaced objs in level
    /// </summary>
    public void AssignButton()
    {
        indexChoosen = Random.Range(0, possibleButtons.Length);
        //Debug.Log("choosen " + indexChoosen);
        for(int objs = 0; objs <= possibleButtons.Length - 1; objs++)
        {
            if(objs == indexChoosen)
            {
                //spawn button 
                possibleButtons[objs].GetComponent<SetButtonOnObj>().SetUpObj(true);
                //Instantiate()
            }
            else
                possibleButtons[objs].GetComponent<SetButtonOnObj>().SetUpObj(false);
        }
    }
   
}

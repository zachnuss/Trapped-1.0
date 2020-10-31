using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetButtonOnObj : MonoBehaviour
{
    [Header("Parent obj that runs whole system")]
    public GameObject buttonObj;
    [Header("Put the possible level objs here")]
    public GameObject[] assetsArray;

    /// <summary>
    /// Dylan Loe
    /// 10-26-2020
    /// 
    /// Runs from ButtonSpawning, if not the button then chooses instead to turn on 
    /// </summary>
    public void SetUpObj(bool button)
    {
        if(button)
        {
            buttonObj.SetActive(true);
            Instantiate(buttonObj, this.transform);
            this.name = "ButtonObj";
            Debug.Log(this.name + " activating true");
        }
        else
        {
            //instantiate asset from array
            assetsArray[Random.Range(0, assetsArray.Length)].SetActive(true);
            this.name = "NotButtonObj_LevelAsset";
            Debug.Log(this.name + " activating false");
        }
    }
}

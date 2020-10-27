using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetButtonOnObj : MonoBehaviour
{
    public GameObject buttonObj;
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
            this.name = "ButtonObj";
        }
        else
        {
            assetsArray[Random.Range(0, assetsArray.Length - 1)].SetActive(true);
            this.name = "NotButtonObj_LevelAsset";
        }
    }
}

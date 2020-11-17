using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[ExecuteInEditMode]
public class SetButtonOnObj : MonoBehaviour
{
   // [TextArea]
   // [Tooltip("This is a tip")]
    //public string Notes = ;

    [Space(4)]
    [Header("Parent obj that runs whole system")]
    public GameObject buttonObj;
    [Header("Put the possible level objs here")]
    public GameObject[] assetsArray;

    [SerializeField, HideInInspector]
    public bool spawnNothing =false;

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
            GameObject key = Instantiate(buttonObj, this.transform);
            key.transform.localPosition = new Vector3(0, 0, 0);
            this.name = "ButtonObj";
            //Debug.Log(this.name + " activating true");
        }
        else
        {
            if (!spawnNothing)
            {
                //instantiate asset from array
                int index = Random.Range(0, assetsArray.Length);
                if (assetsArray[index] != null)
                {
                    assetsArray[index].SetActive(true);
                    this.name = "NotButtonObj_LevelAsset";
                    //Debug.Log(this.name + " activating false");
                }
                else
                {
                    this.name = "NotButtonObj_Empty";
                }
            }
        }
    }

    public void switchBool()
    {
        if (spawnNothing)
        {
           // spawnNothingStr = "Will NOT Spawn Replacement Asset";
            spawnNothing = false;
        }
        else
        {
            spawnNothing = true;
            //spawnNothingStr = "WILL Spawn Replacement Asset";
        }
    }
}

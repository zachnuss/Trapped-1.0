using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGenerator : MonoBehaviour
{
    /// <summary>
    /// Alexander
    /// Updated: 12-1-2020
    /// 
    /// Variables used to hold the light prefab and in which directions to create the light patterns
    /// </summary>
    public GameObject Light;
    public bool ToTheRight;
    public int Columns;
    public int Rows;
    public float LightCreationPause;

    /// <summary>
    /// Alexander
    /// Updated: 12-1-2020
    /// 
    /// Happens before the first frame update
    /// Starts by calling the Coroutine to make the appropriate number of lights going at different times
    /// </summary>
    public void Awake()
    {
        StartCoroutine(createLights(LightCreationPause));
    }

    /// <summary>
    /// Alexander
    /// Updated: 12-1-2020
    /// 
    /// Coroutine function to create the lights at varying times
    /// </summary>
    IEnumerator createLights(float lightPause)
    {
        Debug.Log("Starting light cycle");

        //For loop to go through all the rows for the floor pattern
        for (int row = 0; row < Rows; row++)
        {
            Instantiate(Light, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + row), Quaternion.identity);

            Debug.Log("Creating a light");

            //Waits for the certain amount of time before creating the lights
            yield return new WaitForSeconds(lightPause);
        }
    }
}

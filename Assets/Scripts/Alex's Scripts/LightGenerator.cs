﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGenerator : MonoBehaviour
{
    /// <summary>
    /// Alexander
    /// Updated: 12-3-2020
    /// 
    /// Variables used to hold the light prefab and in which directions to create the light patterns
    /// </summary>
    public GameObject Light;
    public enum Floor { Top, Front, Left, Right, Back, Bottom }; //Sets up an enum to track the side of the cube
    public Floor face = Floor.Top; //Public switch for the level designers to adjust things
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
    /// Updated: 12-3-2020
    /// 
    /// Coroutine function to create the lights at varying times
    /// </summary>
    IEnumerator createLights(float lightPause)
    {
        //For loop to go through all the rows for the floor pattern with if loops to distinquish between them
        
        if(face == Floor.Top) //Top Section
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    Instantiate(Light, new Vector3(this.transform.position.x + col, this.transform.position.y, this.transform.position.z + row), Quaternion.identity);

                    //Waits for the certain amount of time before creating the lights
                    yield return new WaitForSeconds(lightPause);
                }
            }
        }

        if (face == Floor.Front) //Front Section
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    Instantiate(Light, new Vector3(this.transform.position.x + col, this.transform.position.y + row, this.transform.position.z), Quaternion.identity);

                    //Waits for the certain amount of time before creating the lights
                    yield return new WaitForSeconds(lightPause);
                }
            }
        }

        if (face == Floor.Left) //Left Section
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    Instantiate(Light, new Vector3(this.transform.position.x, this.transform.position.y + row, this.transform.position.z - col), Quaternion.identity);

                    //Waits for the certain amount of time before creating the lights
                    yield return new WaitForSeconds(lightPause);
                }
            }
        }

        /*if(face == Floor.Right) //Right Section
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    Instantiate(Light, new Vector3(this.transform.position.x + col, this.transform.position.y, this.transform.position.z + row), Quaternion.identity);

                    //Waits for the certain amount of time before creating the lights
                    yield return new WaitForSeconds(lightPause);
                }
            }
        }

        if(face == Floor.Back) //Back Section
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    Instantiate(Light, new Vector3(this.transform.position.x + col, this.transform.position.y, this.transform.position.z + row), Quaternion.identity);

                    //Waits for the certain amount of time before creating the lights
                    yield return new WaitForSeconds(lightPause);
                }
            }
        }

        if(face == Floor.Bottom) //Bottom Section
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    Instantiate(Light, new Vector3(this.transform.position.x + col, this.transform.position.y, this.transform.position.z + row), Quaternion.identity);

                    //Waits for the certain amount of time before creating the lights
                    yield return new WaitForSeconds(lightPause);
                }
            }
        }
        */
    }

}

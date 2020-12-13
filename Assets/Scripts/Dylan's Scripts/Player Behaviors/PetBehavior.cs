using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dylan Loe
/// 11-2020
/// 
/// Pet makes small movements when in game.
/// </summary>
public class PetBehavior : MonoBehaviour
{
    bool yDir = false;
    float time = 0.5f;
    float speedVar = 0.25f;

    //if the pet is either hornet or bunny - has different speeds and movements
    public bool hornet;

    /// <summary>
    /// Dylan Loe
    /// Updated: 11 2020
    /// 
    /// Starts changes in direction
    /// </summary>
    void Start()
    {
        StartCoroutine(ymove());
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-2020
    /// 
    /// Updates transform to show moevements
    /// </summary>
    void FixedUpdate()
    {
        if (yDir)
            transform.localPosition += new Vector3(0, speedVar * Time.deltaTime, 0);
        else
            transform.localPosition += new Vector3(0, -speedVar * Time.deltaTime, 0);

    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-2020
    /// 
    /// Wait certain time then change directions. Time waiting changes every time called. Different 
    /// pets have different speeds, and time waiting
    /// </summary>
    /// <returns></returns>
    IEnumerator ymove()
    {
        
        yield return new WaitForSeconds(time);

        if (yDir)
        {
            yDir = false;
            if (hornet)
            {
                time = Random.Range(0.3f, 0.7f);
                speedVar = Random.Range(0.15f, 0.25f);
            }
            else
            {
                time = Random.Range(0.4f, 0.5f);
                speedVar = Random.Range(0.1f, 0.2f);
            }
        }
        else
            yDir = true;

        StartCoroutine(ymove());

    }
}

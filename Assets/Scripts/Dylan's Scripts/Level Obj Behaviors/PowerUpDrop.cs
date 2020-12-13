using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dylan Loe
/// Updated: 10-2020
/// 
/// Power up type enum
/// </summary>
public enum powerUpType
{
    damage,
    speed,
    health
};

public class PowerUpDrop : MonoBehaviour
{
    [Header("What type of powerup")]
    public powerUpType type;

    public bool timer = true;
    [Header("How long powerup will last")]
    public int powerUpDuration = 10;

    bool yDir = false;

    void Start()
    {
        StartCoroutine(ymove());
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-2020
    /// 
    /// This powerup constantly moves up and down as well as continueously rotates at constant speed.
    /// </summary>
    void FixedUpdate()
    {

            if (yDir)
                transform.localPosition += new Vector3(0, 0.25f * Time.deltaTime, 0);
            else
                transform.localPosition += new Vector3(0, -0.25f * Time.deltaTime, 0);

            transform.Rotate(0, 0.25f, 0, Space.Self);
    }

    /// <summary>
    /// Dylan loe
    /// Updated: 10-20-2020
    /// 
    /// Change direciton every second to give floating feel in scene
    /// </summary>
    IEnumerator ymove()
    {
        yield return new WaitForSeconds(1.0f);
        if (yDir)
            yDir = false;
        else
            yDir = true;

        StartCoroutine(ymove());

    }
}

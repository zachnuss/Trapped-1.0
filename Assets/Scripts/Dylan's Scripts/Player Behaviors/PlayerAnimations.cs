using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animatorTop;
    public Animator animatorBottom;
    //may need to set animaotrs manually


    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Sets animator for player
    /// </summary>
    void Start()
    {
        //animatorTop = GetComponent<Animator>();
    }

    //top
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets top animator to isMoving
    /// </summary>
    public void IsMovingTop()
    {
        animatorTop.SetBool("isIdling", false);
        //animatorTop.SetBool("Death", false);
        animatorTop.SetBool("isMoving", true);
        animatorTop.SetBool("isFiring", false);
        //Debug.Log("Set Moving Top Animation");
    }
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets top animator to isIdling
    /// </summary>
    public void isIdlingTop()
    {
        animatorTop.SetBool("isIdling", true);
        //animatorTop.SetBool("Death", false);
        animatorTop.SetBool("isMoving", false);
        animatorTop.SetBool("isFiring", false);
        //Debug.Log("Set Idling Top Animation");
    }
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets top animator to isFiring
    /// </summary>
    public void isFiringTop()
    {
        animatorTop.SetBool("isIdling", false);
       // animatorTop.SetBool("Death", false);
        animatorTop.SetBool("isMoving", false);
        animatorTop.SetBool("isFiring", true);
        //Debug.Log("Start Firing Animation");
    }
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets top animator to isDying
    /// </summary>
    public void DeathAnimatorTop()
    {
        animatorTop.SetBool("isIdling", false);
        //animatorTop.SetBool("Death", true);
        animatorTop.SetBool("isMoving", false);
        animatorTop.SetBool("isFiring", false);
        //Debug.Log("Death");
    }

    //bottom legs 
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets bottom animator to isMovingBottom
    /// </summary>
    public void IsRunningBut()
    {
        animatorBottom.SetBool("isIdling", false);
        animatorBottom.SetBool("Death", false);
        animatorBottom.SetBool("isRunning", true);

    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets top animator to isIdlingBottom
    /// </summary>
    public void IsIdlingBut()
    {
        animatorBottom.SetBool("isIdling", true);
        animatorBottom.SetBool("Death", false);
        animatorBottom.SetBool("isRunning", false);
    }
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets top animator to isDyingBottom
    /// </summary>
    public void DeathAnimatorBut()
    {
        animatorBottom.SetBool("isIdling", false);
        animatorBottom.SetBool("Death", true);
        animatorBottom.SetBool("isRunning", false);
    }
}

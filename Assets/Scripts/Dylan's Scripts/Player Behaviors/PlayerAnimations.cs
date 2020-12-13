using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    /// <summary>
    /// Initially there were gunna be a top half of the body for aiming and lookign and a bottom half used 
    /// for the movement. The controller can have the player run one diretion and shoot another. Due to constraits 
    /// made by not getting efficent rigs and player avatars, there will only be one animator per character.
    /// </summary>
    public Animator animatorTop;
    public Animator animatorBottom;
    //may need to set animaotrs manually

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets top animator to isMoving
    /// </summary>
    public void IsMovingTop()
    {
        animatorTop.SetBool("isIdling", false);
        animatorTop.SetBool("isMoving", true);
        animatorTop.SetBool("isFiring", false);
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
        animatorTop.SetBool("isMoving", false);
        animatorTop.SetBool("isFiring", false);
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
        animatorTop.SetBool("isMoving", false);
        animatorTop.SetBool("isFiring", true);
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
        animatorTop.SetBool("isMoving", false);
        animatorTop.SetBool("isFiring", false);
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

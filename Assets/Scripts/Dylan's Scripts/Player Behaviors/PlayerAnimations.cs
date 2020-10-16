using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animatorTop;
    public Animator animatorBottom;
    //may need to set animaotrs manually


    // Start is called before the first frame update
    void Start()
    {
        //animatorTop = GetComponent<Animator>();
    }

    //top
    public void IsMovingTop()
    {
        animatorTop.SetBool("isIdling", false);
        animatorTop.SetBool("Death", false);
        animatorTop.SetBool("isMoving", true);
        animatorTop.SetBool("isFiring", false);
        Debug.Log("Set Moving Top Animation");
    }
    public void isIdlingTop()
    {
        animatorTop.SetBool("isIdling", true);
        animatorTop.SetBool("Death", false);
        animatorTop.SetBool("isMoving", false);
        animatorTop.SetBool("isFiring", false);
        Debug.Log("Set Idling Top Animation");
    }
    public void isFiringTop()
    {
        animatorTop.SetBool("isIdling", false);
        animatorTop.SetBool("Death", false);
        animatorTop.SetBool("isMoving", false);
        animatorTop.SetBool("isFiring", true);
        Debug.Log("Start Firing Animation");
    }
    public void DeathAnimatorTop()
    {
        animatorTop.SetBool("isIdling", false);
        animatorTop.SetBool("Death", true);
        animatorTop.SetBool("isMoving", false);
        animatorTop.SetBool("isFiring", false);
        Debug.Log("Death");
    }

    //bottom legs 
    public void IsRunningBut()
    {
        animatorBottom.SetBool("isIdling", false);
        animatorBottom.SetBool("Death", false);
        animatorBottom.SetBool("isRunning", true);

    }
    public void IsIdlingBut()
    {
        animatorBottom.SetBool("isIdling", true);
        animatorBottom.SetBool("Death", false);
        animatorBottom.SetBool("isRunning", false);
    }
    public void DeathAnimatorBut()
    {
        animatorBottom.SetBool("isIdling", false);
        animatorBottom.SetBool("Death", true);
        animatorBottom.SetBool("isRunning", false);
    }
}

/*
 * EnemyAnimation.cs
 * Author: Christian Mullins
 * Summary: This script handles all the states for animation in enemies.
 */
using UnityEngine;

public class EnemyAnimations : MonoBehaviour {
    private Animator _animator;
    //shielded guard fix for shield flipping
    private Transform _shield;
    
    void Start() {
        _animator = GetComponent<Animator>();
        /* TO DO
         *      -Create code that will adjust speed when appropriate (Menu scene vs in-game)
         *          --CAN'T ALTER AS THESE VALUES ARE GETTERS ONLY
         *      -May need to code for various animator factors based on if CommonGuard or ShieldedGuard
         */
    }

    /**     ANIMATION STATE FUNCTIONS       **/
    /*
     * Animation bools:
     *      HallwayBot:
     *          isIdle
     *          isMoving
     *      CommonGuards:
     *          isWalking
     *          isShooting
     *          isIdle
     *      ShieldedGuard:
     *          isIdle
     *          isWalking
     *          isCharging
     */
    //SET IDLE STATE FUNCTIONS
    public void isIdle_HallwayBot() {
        _animator.SetBool("isIdle", true);
        _animator.SetBool("isMoving", false);
    }
    public void isIdle_CommonGuard() {
        if (!_animator.GetBool("isIdle")) {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isShooting", false);
            _animator.Play("isIdle");
            //print("isAnimating: Idle");
        }
    }
    public void isIdle_ShieldedGuard() {
        _animator.SetBool("isIdle", true);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isCharging", false);
        _animator.Play("isIdle");
    }
    //SET WALKING STATE FUNCTIONS
    public void isWalking_CommonGuard() {
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", true);
        _animator.SetBool("isShooting", false);
        _animator.Play("isWalking");
            //print("isAnimating: Walking");
    }
    public void isWalking_ShieldedGuard() {
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", true);
        _animator.SetBool("isCharging", false);
        _animator.Play("isWalking");
    }
    public void isMoving_HallwayBot() {
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isMoving", true);
    }
    //SET RUNNING STATE FUNCTIONS
    public void isCharging_ShieldedGuard() {
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isCharging", true);
    }
    //SET SHOOTING STATE FUNCTIONS
    public void isShooting_CommonGuard() {
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isShooting", true);
        //print("isAnimating: isShooting");
    }
}

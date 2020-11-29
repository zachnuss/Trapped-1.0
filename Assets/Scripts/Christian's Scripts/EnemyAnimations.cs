/*
 * EnemyAnimation.cs
 * Author: Christian Mullins
 * Summary: This script handles all the states for animation in enemies.
 */

using UnityEngine;

public class EnemyAnimations : MonoBehavior {
    private Animator _animator;

    void Awake() {
        _animator = GetComponent(Animator);
    }

    /**     ANIMATION STATE FUNCTIONS       **/
    /*
     * Animation bools:
     *  `   HallwayBot:
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
        _animator.SetBool("isIdle", true);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isShooting", false);
    }
    public void isIdle_ShieldedGuard() {
        _animator.SetBool("isIdle", true);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isCharging", false);
    }
    //SET WALKING STATE FUNCTIONS
    public void isWalking_CommonGuard() {
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", true);
        _animator.SetBool("isShooting", false);
    }
    public void isWalking_ShieldedGuard() {
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", true);
        _animator.SetBool("isCharging", false);
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
    }
}
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
    public void isIdle_CommonGuard() {
        if (!_animator.GetBool("isIdle")) {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isShooting", false);
        }
        _animator.Play("isIdle");
    }
    public void isIdle_ShieldedGuard() {
        if (!_animator.GetBool("isIdle")) {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isCharging", false);
        }
        _animator.Play("isIdle");
    }
    //SET WALKING STATE FUNCTIONS
    public void isWalking_CommonGuard() {
        if (!_animator.GetBool("isWalking")) {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isShooting", false);
        }
        _animator.Play("isWalking");
    }
    public void isWalking_ShieldedGuard() {
        if (!_animator.GetBool("isWalking")) {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isCharging", false);
        }
        _animator.Play("isWalking");
    }
    //SET RUNNING STATE FUNCTIONS
    public void isCharging_ShieldedGuard() {
        if (!_animator.GetBool("isCharging")) {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isCharging", true);
        }
        _animator.Play("isCharging");
    }
    //SET SHOOTING STATE FUNCTIONS
    public void isShooting_CommonGuard() {
        if (!_animator.GetBool("isShooting")) {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isShooting", true);
        }
        _animator.Play("isShooting");
    }
}

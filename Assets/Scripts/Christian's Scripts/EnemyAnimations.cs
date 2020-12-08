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
     *          isIdle
     *          isWalking
     *          isShooting
     *      ShieldedGuard:
     *          isIdle
     *          isWalking
     *          isCharging
     */
     //COMMONGUARD FUNCTIONS
    public void isIdle_CommonGuard() {
        if (!_animator.GetBool("isIdle")) {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isShooting", false);
            _animator.StopPlayback();
        }
        _animator.Play("isIdle");
    }

    public void isWalking_CommonGuard() {
        if (!_animator.GetBool("isWalking")) {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isShooting", false);
            _animator.StopPlayback();
            
        }
        _animator.Play("isWalking");
    }

    public void isShooting_CommonGuard() {
        if (!_animator.GetBool("isShooting")) {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isShooting", true);
            _animator.StopPlayback();
        }
        _animator.Play("isShooting");
    }

    //SHIELDEDGUARD FUNCTIONS
    public void isIdle_ShieldedGuard() {
        if (!_animator.GetBool("isIdle")) {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isCharging", false);
            _animator.StopPlayback();
        }
        _animator.Play("isIdle");
    }

    public void isWalking_ShieldedGuard() {
        if (!_animator.GetBool("isWalking")) {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isCharging", false);
            _animator.StopPlayback();
        }
        _animator.Play("isWalking");
    }

    public void isCharging_ShieldedGuard() {
        if (!_animator.GetBool("isCharging")) {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isCharging", true);
            _animator.StopPlayback();
        }
        _animator.Play("isCharging");
    }
}

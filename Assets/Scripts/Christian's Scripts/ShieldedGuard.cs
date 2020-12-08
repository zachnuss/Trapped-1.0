/*
 * Author: Christian Mullins
 * Summary: Inheritance of the CommonGuard that will omit shooting the player.
 */
 //defining NOTSHOOTER prevents the parent script from running the shooting
 //portion of the script
#define NOTSHOOTER
using UnityEngine;

public class ShieldedGuard : CommonGuard {
    ///private
    private float _timeForRest = 4f;
    private float _restTimer = 0f;
    private float _timeStunned = 2.75f;
    private bool _isBashing = false;
    private bool _isStunned = false;

    //incorporate sprinting into this script
    protected override void Update() {
        //if stunned, do nothing
        if (_isStunned) return;
        ///check before movement
        if (_myBehavior != Behavior.Idle && _myBehavior != Behavior.TrackPlayer
            && !_isTrackingPlayer && !_isBashing) {
            _move(_moveDir);
        }
        else if (_isTrackingPlayer && !_isBashing) {
            //override _move() because the enemy will be too focussed on
            //the player to turn around when hitting the wall
            _trackPlayer();

            //raytrace to get if I'm in front of the player
            _myBehavior = Behavior.TrackPlayer;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, 
                transform.TransformDirection(_fwdDirGO.transform.localPosition),
                out hit, playerRangeCheck)) {
                if (hit.transform.tag == "Player") {
                    //shield bash
                    _isTrackingPlayer = false;
                    _isBashing = true;
                    Vector3 vecToPlayer = _playerGO.transform.position - transform.position;
                    _moveDir = transform.TransformDirection(_fwdDirGO.transform.localPosition);
                    _moveDir = _moveDir.normalized;
                    _moveDir.y = 0f;
                }
            }
        }
        if (_isBashing) {
            _shieldBash();
        }
    }
    
    //FixedUpdate() reserved for animation states
    private void LateUpdate() {
        ///set animation states when necessary
        if (_myBehavior == Behavior.Idle) {
            animationState = EnemyAnimation.Idle;
            _animations.isIdle_ShieldedGuard();
        }
        else if (!_canSprint) {
            //the enemy is currently sprinting and not shooting
            animationState = EnemyAnimation.Running;
            _animations.isCharging_ShieldedGuard();
        }
        else {
            //default state that shouldn't be interupted by shooting
            animationState = EnemyAnimation.Walking;
            _animations.isWalking_ShieldedGuard();
        }
    }

    //Once facing the player, maintain _moveDir and run until "Wall" is hit
    private void _shieldBash() {
        //if wall is hit, stun
        if (_isEnemyFacingWall()) {
            _isBashing = false;
            speed = _storeRegSpeed;
            Invoke("_resetBehaviors", _timeStunned);
            _myBehavior = Behavior.Idle;
            InvokeRepeating("_changeBehavior", _timeStunned, rateOfBehaviorChange);
            _isStunned = true;
            Invoke("_stunTimer", _timeStunned);
            return;
        }

        //run directly toward player
        speed = _storeRegSpeed * 1.75f;
        if (Vector3.Distance(transform.position, _playerGO.transform.position)
            > 0.5f) {
                transform.localPosition += _moveDir * speed * Time.fixedDeltaTime;
        }
    }

    private void _stunTimer() {
        _isStunned = false;
    }

    //debugging function for using Invoke()
    private void _invokeDebug() {
        Debug.Log("INVOKED");
    }
}
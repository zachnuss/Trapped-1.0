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
        /*
        //check direction to player
        Direction dirOfPlayer = Direction.NULL;
        if (!_isBashing && _myBehavior != Behavior.Idle) {
            dirOfPlayer = _isPlayerInRange();
        }
        if (dirOfPlayer != Direction.NULL && !_isBashing) {
            //change behavior
            _myBehavior = Behavior.TrackPlayer;
            //suspend changing behavior and set trackingTimer
            if (!_isTrackingPlayer) {
                CancelInvoke("_changeBehavior");
                _isTrackingPlayer = true;
            }
            _trackingTimer = Time.time + playerSearchTimer;
            //if facing the player, begin charging
            if (dirOfPlayer == Direction.Forward) {
                _isBashing = true;
                _isTrackingPlayer = false;
            }
        }
        else if (_hasLostPlayer()) {
            //resume change behavior
            if (_isTrackingPlayer) {
                _resetBehaviors();
                InvokeRepeating("_changeBehavior", 1.75f, rateOfBehaviorChange);
                _isTrackingPlayer = false;
            }
        }
        */
        /**         OLD SHIT ABOVE      **/
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
                 //shield bash
                _isTrackingPlayer = false;
                _isBashing = true;
                _moveDir = transform.TransformDirection(_fwdDirGO.transform.localPosition);
                _moveDir = _moveDir.normalized;
            }
        }
        if (_isBashing) {
            _shieldBash();
        }


        ///set animation states when necessary
        if (_myBehavior == Behavior.Idle) {
            animationState = EnemyAnimation.Idle;
        }
        else if (!_canSprint) {
            //the enemy is currently sprinting and not shooting
            animationState = EnemyAnimation.Running;
        }
        else {
            //default state that shouldn't be interupted by shooting
            animationState = EnemyAnimation.Walking;
        }
    }

    //Once facing the player, maintain _moveDir and run until "Wall" is hit
    private void _shieldBash() {
        //run directly toward player
        speed = _storeRegSpeed * 1.75f;
        if (Vector3.Distance(transform.position, _playerGO.transform.position)
            > 0.5f) {
                transform.localPosition += _moveDir * speed * Time.fixedDeltaTime;
        }

        //if wall is hit, stun
        if (_isEnemyFacingWall()) {
            _isBashing = false;
            speed = _storeRegSpeed;
            Invoke("_resetBehaviors", _timeStunned);
            _myBehavior = Behavior.Idle;
            InvokeRepeating("_changeBehavior", _timeStunned, rateOfBehaviorChange);
            _isStunned = true;
            Invoke("_stunTimer", _timeStunned);
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
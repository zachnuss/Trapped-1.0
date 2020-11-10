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

    //incorporate sprinting into this script
    protected override void Update() {
        //check direction to player
        Direction dirOfPlayer = _isPlayerInRange();
        if (dirOfPlayer != Direction.NULL) {
            //change behavior
            _myBehavior = Behavior.TrackPlayer;
            ///suspend changing behavior and set trackingTimer
            if (!_isTrackingPlayer) {
                CancelInvoke("_changeBehavior");
                _isTrackingPlayer = true;
            }
            _trackingTimer = Time.time + playerSearchTimer;
            //sprint
            if (_canSprint && Time.time > _restTimer) {
                //increase speed for tracking
                speed = _storeRegSpeed * 1.75f;
                Invoke("_sprintCoolDown", 2.0f); //returns speed back when invoked
                _canSprint = false;
                _restTimer = Time.time + _timeForRest;
            }
        }
        else if (_hasLostPlayer()) {
            ///resume change behavior
            if (_isTrackingPlayer) {
                _resetBehaviors();
                InvokeRepeating("_changeBehavior", 1.75f, rateOfBehaviorChange);
                _isTrackingPlayer = false;
            }
        }

        ///check before movement
        if (_myBehavior != Behavior.Idle && _myBehavior != Behavior.TrackPlayer
            && !_isTrackingPlayer) {
            _move(_moveDir);
        }
        else if (_isTrackingPlayer) {
            //override _move() because the enemy will be too focussed on
            //the player to turn around when hitting the wall
            _trackPlayer();
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


    //call as invoke so the boolean will get swapped back
    private void _sprintCoolDown() {
        speed = _storeRegSpeed;
        _canSprint = true;
    }
}
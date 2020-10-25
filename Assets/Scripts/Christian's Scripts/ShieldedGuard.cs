/*
 * Author: Christian Mullins
 * Summary: Inheritance of the CommonGuard that will omit shooting the player.
 */

 //defining NOTSHOOTER prevents the parent script from running the shooting
 //portion of the script
#define NOTSHOOTER

public class ShieldedGuard : CommonGuard {
    //incorporate sprinting into this script
    protected override void Update() {
        //check direction to player
        Direction dirOfPlayer = _isPlayerInRange();
        if (dirOfPlayer != Direction.NULL) {
            //change behavior
            _myBehavior = Behavior.TrackPlayer;

            //move in the desired direction
            _turnThisDirection(dirOfPlayer);

            ///suspend changing behavior
            CancelInvoke("_changeBehavior");

            ////sprint
            //if (_canSprint) {
            //    //increase speed for tracking
            //    speed = _storeRegSpeed * 1.25f;
            //    Invoke("_sprintCoolDown", 2.5f); //returns speed back when invoked
            //    _canSprint = false;
            //}
        }
        else {
            ///resume change behavior
            if (_isTrackingPlayer) {
                InvokeRepeating("_changeBehavior", 0f, rateOfBehaviorChange);
            }

            _isTrackingPlayer = false;
        }

        ///check before movement
        if (_myBehavior != Behavior.Idle) {
            _move(_moveDir);
        }
    }


    //call as invoke so the boolean will get swapped back
    private void _sprintCoolDown() {
        speed = _storeRegSpeed;
        _canSprint = true;
    }
}

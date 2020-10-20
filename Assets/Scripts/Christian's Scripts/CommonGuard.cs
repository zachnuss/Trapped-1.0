/*
 * Author: Christian Mullins
 * Summary: First instance of inheritance of the BaseEnemy class.
 */
using UnityEngine;

/**
 * TO DO:
 *      NEW ALGORITHM:
 *      - 2 enemy states, looking and tracking/attacking
 *      - When tracking/attacking, just run at the player, don't mess with
 *          your 4 way directional system.
 *      - If the enemy losses sight of the player, then reset your system
 *          (rotation on child dir position.
 *              - resetting the system should occur when the player leaves your
 *              level/face. Or use a timer (for time not tracked, or time not
 *              finding player with raycast.
 * 
 *      Things to mention to Leads:
 *      TIME CONSUMINGs
 *      - Running offscreen bug will need some time to actually be debugged.
 *          - Dirty bug fix can fix this worst case
 *      - Tracking and shooting player can possibly be improved.
 */ 

public class CommonGuard : BaseEnemy {

    [Header("How far away can I see the player?")]
    public float playerRangeCheck = 10.0f;

    ///protected
    protected bool _canSprint = true;
    protected bool _isTrackingPlayer = false;
    protected float _storeRegSpeed;
    ///private
    private GameObject _leftDirGO, _rightDirGO;

    ///variables necessary if this instance shoots
    //private bool _doesEnemyShoot = false;
    public GameObject fwdDirGO { get { return _fwdDirGO; } }

    protected void Awake() {
        //store start speed for sprinting
        _storeRegSpeed = speed;

        //assign directional children
        int childrenNum = transform.childCount;
        for (int i = 0; i < childrenNum; ++i) {
            GameObject assigningGO = transform.GetChild(i).gameObject;
            //assign gameObject to a child
            if (assigningGO.name == "LeftChild") { 
                _leftDirGO = assigningGO;
            }
            if (assigningGO.name == "RightChild") {
                _rightDirGO = assigningGO;
            }
            //front child is in the BaseEnemy
        }
    }

    protected override void Update() {
        //check if the player is in front of me or to the left or right
        Direction dirOfPlayer = _isPlayerInRange();
        if (dirOfPlayer != Direction.NULL) {
            //change behavior
            _myBehavior = Behavior.TrackPlayer;

            //move in the desired direction
            _turnThisDirection(dirOfPlayer);

            ///suspend changing behavior
            CancelInvoke("_changeBehavior");

            //check for conditions, then shoot and reset vals
            //NOTSHOOTER is only defined in children who
#if !NOTSHOOTER
            if (TryGetComponent<EnemyShooting>(out EnemyShooting eS)) {
                if (eS.canShootPlayer && dirOfPlayer == Direction.Forward) {
                    eS.shootPlayer();
                    eS.shootTimer = Time.time + eS.shootCoolDown;
                    eS.canShootPlayer = false;
                }
            }
            else {
                Debug.LogError(name + ": doesn't contain EnemyShooting.cs " +
                             "or NOTSHOOTER is not defined in child class.");
            }
#endif
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

    //if the player is never hit, then return Direction.NULL
    protected Direction _isPlayerInRange() {
        Direction dirOfPlayer = Direction.NULL;
        float castDist = playerRangeCheck;
        RaycastHit hit;

        //get location of child GOs

        Vector3 lookForward = _fwdDirGO.transform.position - transform.position;
        Vector3 lookRight = _rightDirGO.transform.position - transform.position;
        Vector3 lookLeft = _leftDirGO.transform.position - transform.position;

        //get approximate width of the player
        Vector3 rightHip = (_rightDirGO.transform.position + transform.position) / 2f;
        Vector3 leftHip = (_leftDirGO.transform.position + transform.position) / 2f;

        //look forward
        if ((Physics.Raycast(transform.position, lookForward, out hit, castDist)
            || Physics.Raycast(leftHip, lookForward, out hit, castDist)
            || Physics.Raycast(rightHip, lookForward, out hit, castDist)) 
            && hit.transform.tag == "Player") {
            dirOfPlayer = Direction.Forward;
        }
        //look left
        else if (Physics.Raycast(transform.position, lookLeft, out hit, castDist)
            && hit.transform.tag == "Player") {
            dirOfPlayer = Direction.Left;
        }
        //look right
        else if (Physics.Raycast(transform.position, lookRight, out hit, castDist)
            && hit.transform.tag == "Player") {
            dirOfPlayer = Direction.Right;
        }

        ///draw raycast in space to debug
        //draw forward
        Debug.DrawRay(transform.position, lookForward.normalized, Color.black, 0.2f, false);
        Debug.DrawRay(rightHip, lookForward.normalized, Color.red, 0.2f, false);
        Debug.DrawRay(leftHip, lookForward.normalized, Color.blue, 0.2f, false);
        //draw left
        //Debug.DrawRay(transform.position, lookLeft.normalized, Color.red, 0.2f, false);
        //draw right
        //Debug.DrawRay(transform.position, lookRight.normalized, Color.blue, 0.2f, false);

        return dirOfPlayer;
    }

    /** CURRENT TRACKING FUNCTION CAN BE SWITCHED TO SEARCHING **/
    ///function for tracking the player
    ///     This will "override" the current movement function in Update
    protected void _trackPlayer() {

    }
    ///bool function checking if enemy should reset their state
    protected bool _hasLostPlayer() {
        return false; //standin value
    }
    ///function to reset enemy state to searching
    protected void _resetBehaviors() {
        //reset rotation
        Vector3 resetVec = -transform.localEulerAngles;
        transform.Rotate(resetVec, Space.Self);
        //reset _moveDir
        Vector3 newMoveDir = (_fwdDirGO.transform.position 
                                - transform.position).normalized;
        _moveDir = newMoveDir;
        //change _isTracking bool to reset InvokeRepeating in Update()
        _isTrackingPlayer = false;
        //change behavior
        _myBehavior = Behavior.ChangeDirection;
    }

    ///Dirty bug fix for running off the level
    ///     periodically check raycast below, if nothing is hit by the raycast
    ///     (this means that the enemy is off the map) reset to the start playing
    ///     position on the level
    private void _resetEnemy() {
        //change my position back to start position

        //reset everything else
        _resetBehaviors();
    }
}
/*
 * Author: Christian Mullins
 * Summary: First instance of inheritance of the BaseEnemy class.
 */
using UnityEngine;
//using System; //remove later

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
 */ 

public class CommonGuard : BaseEnemy {

    [Header("How far away can I see the player?")]
    public float playerRangeCheck = 10.0f;
    public float playerSearchTimer = 15.0f;

    ///protected
    protected bool _canSprint = true;
    protected bool _isTrackingPlayer = false;
    protected float _storeRegSpeed;
    ///private
    private GameObject _leftDirGO, _rightDirGO;
    private float _trackingTimer;

    ///variables necessary if this instance shoots
    //private bool _doesEnemyShoot = false;
    public GameObject fwdDirGO { get { return _fwdDirGO; } }


    /**     PUBLIC FUNCTIONS    */
    public override void takeDamage(GameObject player) {
        //take health away
        health -= player.GetComponent<PlayerMovement>().damage;
        ///CommonGuard will fight back as the player is "found"
        if (!_isTrackingPlayer) {
            CancelInvoke("_changeBehavior");
            _isTrackingPlayer = true;
        }
        _trackingTimer = Time.time + playerSearchTimer;

        //did the enemy die?
        if (health < 1) {
            health = 0;
            //give score to player
            player.GetComponent<PlayerMovement>().playerData.AddScore(pointValue);
            player.GetComponent<PlayerMovement>().playerData.TrackEnemyScore(pointValue);
            player.GetComponent<PlayerMovement>().playerData.TrackEnemyKills(1);
            if (Random.Range(0f, 100f) <= 5)
            {
                Debug.Log("Currency Test Complete!");
                Instantiate(specialCoin, this.transform.position, this.transform.rotation);
            }
            //Debug.Log("Enemy killed, " + pointValue + " points added to PlayerData.");
            if (doubleDamageMod) {
                player.GetComponent<PlayerMovement>().playerData.AddScore(pointValue);
            }
            //destroy enemy last to avoid bugs
            Destroy(gameObject);
        }
    }


    /**     PROTECTED FUNCTIONS     */
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
        if (_isTrackingPlayer) {
            //Debug.Log("TRACKING THE PLAYER NOW");
        }
        //start looking fo player
        //check if the player is in front of me or to the left or right
        Direction dirOfPlayer = _isPlayerInRange();
        if (dirOfPlayer != Direction.NULL) {
            //change behavior
            _myBehavior = Behavior.TrackPlayer;

            ///suspend changing behavior and set trackingTimer
            if (!_isTrackingPlayer) {
                //move in the desired direction
                //_turnThisDirection(dirOfPlayer);
                CancelInvoke("_changeBehavior");
                _isTrackingPlayer = true;
            }
            _trackingTimer = Time.time + playerSearchTimer;

            //check for conditions, then shoot and reset vals
            //NOTSHOOTER is only defined in children scripts who won't shoot
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
        else if (_hasLostPlayer()) {
            //the player has not be detected and we're not tracking
            ///resume change behavior
            if (_isTrackingPlayer) {
                _resetBehaviors();
                InvokeRepeating("_changeBehavior", 1.75f, rateOfBehaviorChange); 
                _isTrackingPlayer = false;
            }
        }

        ///check before movement
        if (_myBehavior != Behavior.Idle && !_isTrackingPlayer) {
            _move(_moveDir);
        }
        else if (_isTrackingPlayer) {
            //override _move() because the enemy will be too focuessed on
            //the player to turn around when hitting the wall
            _trackPlayer(dirOfPlayer);
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

        //look forward
        if (Physics.Raycast(transform.position, lookForward, out hit, castDist)
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
        //Debug.DrawRay(transform.position, lookForward.normalized, Color.black, 0.2f, false);
        //Debug.DrawRay(rightHip, lookForward.normalized, Color.red, 0.2f, false);
        //Debug.DrawRay(leftHip, lookForward.normalized, Color.blue, 0.2f, false);
        //draw left
        //Debug.DrawRay(transform.position, lookLeft.normalized, Color.red, 0.2f, false);
        //draw right
        //Debug.DrawRay(transform.position, lookRight.normalized, Color.blue, 0.2f, false);

        return dirOfPlayer;
    }

    //will operate the exact same as the _move function, but won't turn around
    //when facing the wall
    protected void _trackPlayer(Direction dirToPlayer) {
        //apply rotation to face the player
        Vector3 vecToPlayer = (playerGO.transform.position
                                - transform.position).normalized;
        //calculate new _moveDir
        Vector3 curMoveDir = (_fwdDirGO.transform.position
                                - transform.position).normalized;
        _moveDir = curMoveDir;

        float yRotLerp = Vector3.SignedAngle(curMoveDir, vecToPlayer, Vector3.up);
        yRotLerp = Mathf.LerpUnclamped(0f, yRotLerp, Time.deltaTime * 4.5f);
        transform.Rotate(0f, yRotLerp, 0f, Space.Self);

        //set spacer between the enemy and player, value will change based on
        //what enemy is attacking
        float spaceBetween = 1.5f;
#if NOTSHOOTER
        spaceBetween = 0.75f;
#endif

        if (!_isEnemyFacingWall()) {
            //Enemies that don't shoot will simply ram in to the player
            //raycast to check if I'm too close to the player
            RaycastHit hit;
            if (!(Physics.Raycast(transform.position, vecToPlayer, out hit, spaceBetween)
                && hit.transform.tag == "Player")) {
                //move the player
                transform.position += _moveDir * speed * Time.deltaTime;
            }
        }
    }
    ///bool function checking if enemy should reset their state
    protected bool _hasLostPlayer() {
        bool isLost = false;
        //check if the player has moved out of a specified distance threshold
        //and check if the enemy hasn't been in range for 5f seconds
        if (Time.time > _trackingTimer ||
            playerRangeCheck < Vector3.Distance(transform.position, 
                                                playerGO.transform.position)) {
            isLost = true;
        }
        return isLost;
    }

    ///function to reset enemy state to searching
    protected void _resetBehaviors() {
        //reset rotation (should be constrained to the y-axis)
        transform.Rotate(-transform.localEulerAngles, Space.Self);
        //reset _moveDir
        Vector3 newMoveDir = (_fwdDirGO.transform.position
                                - transform.position).normalized;
        _moveDir = newMoveDir;
        //change _isTracking bool to reset InvokeRepeating in Update()
        _isTrackingPlayer = false;
        //change behavior
        _myBehavior = Behavior.Idle;
    }

}
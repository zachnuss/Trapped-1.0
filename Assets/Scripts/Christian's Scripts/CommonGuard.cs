﻿/*
 * Author: Christian Mullins
 * Summary: First instance of inheritance of the BaseEnemy class.
 */
using UnityEngine;

public class CommonGuard : BaseEnemy {

    [Header("How far away can I see the player?")]
    public float playerRangeCheck = 20.0f;
    public float playerSearchTimer = 15.0f;

    ///protected
    protected bool _canSprint = true;
    protected bool _isTrackingPlayer = false;
    protected float _storeRegSpeed;
    ///private
    private GameObject _leftDirGO, _rightDirGO;
    private float _trackingTimer;
    private Transform _startTransform;

    ///variables necessary if this instance shoots
    //private bool _doesEnemyShoot = false;
    public GameObject fwdDirGO { get { return _fwdDirGO; } }
    public bool isTrackingPlayer { get { return _isTrackingPlayer; } }


    /**     PUBLIC FUNCTIONS    */
    public override void takeDamage(GameObject player) {
        //take health away
        health -= player.GetComponent<PlayerMovement>().damage;
        ///CommonGuard will fight back as the player is "found"
        if (!_isTrackingPlayer) {
            /**
             * BUG TO FIX:
             *      --Guard "wing flapping" upon tracking
             *          -When a guard begins tracking the player they can stutter
             *              in their rotation.
             *        -Possible fixes
             *          -Mess with the interpolation values
             *          -Switch to another interpolation type
             *          -adjusting clamping values on rotation***
             *              --make a test case for finding where the enemy is
             *                  facing in relation to the player.
             */ 
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
        //store startting values
        _storeRegSpeed = speed;
        _startTransform = transform;

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
            //front child is stored in the BaseEnemy
        }
    }

    protected override void Update() {
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
                Debug.Log("PLAYER LOST");
            }
        }

        ///check before movement
        if (_myBehavior != Behavior.Idle && _myBehavior != Behavior.TrackPlayer
            && !_isTrackingPlayer) {
            _move(_moveDir);
        }
        else if (_isTrackingPlayer) {
            //override _move() because the enemy will be too focuessed on
            //the player to turn around when hitting the wall
            _trackPlayer();
            //Debug.Log("TRACKING THE PLAYER NOW");
        }
    }
    /*
    private void FixedUpdate()
    {
        if (_isTrackingPlayer) {
            _trackPlayer();
        }
    }
    */
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
    protected void _trackPlayer() {
        //apply rotation to face the player
        Vector3 vecToPlayer = (_playerGO.transform.position
                                - transform.position);
        //calculate new _moveDir
        Vector3 curMoveDir = (_fwdDirGO.transform.position
                                - transform.position);
        _moveDir = curMoveDir.normalized;
        //_moveDir = _moveDir.normalized;
        //set spacer between the enemy and player, value will change based on
        //what enemy is attacking
        float spaceBetween;
#if NOTSHOOTER
        spaceBetween = 0.75f;
#else
        spaceBetween = 1.5f;
#endif

        if (!_isEnemyFacingWall()) {
            //Enemies that don't shoot will simply ram in to the player
            //raycast to check if I'm too close to the player
            RaycastHit hit;
            bool hitSomething = Physics.Raycast(transform.position, 
                                                vecToPlayer, out hit, spaceBetween);
            if (!(hitSomething && hit.transform.tag == "Player")) {
                //move the player
                transform.position += _moveDir * speed * Time.fixedDeltaTime;
                //return;
            }
        }

        //handle differently depending on how large the rotational value is
        //originally signed angle

        float yRotLerp = Vector3.SignedAngle(curMoveDir, vecToPlayer, Vector3.up);
        //Vector3 rotTo = new Vector3(0f, yRotLerp * Time.fixedDeltaTime, 0f) + transform.position;
        //if the lerp val is negative, account for this in the minLerpVal
        //yRotLerp = Mathf.Lerp(0f, yRotLerp, 0.4f);

        //yRotLerp *= Time.fixedDeltaTime;
        if (Mathf.Abs(yRotLerp) < 60f) {
            yRotLerp *= Time.fixedDeltaTime;
        }
        else {
            yRotLerp *= Time.fixedDeltaTime * 1.75f;
        }

        transform.Rotate(Vector3.up, yRotLerp, Space.Self);
        //Debug.Log("yRotLerp: " + yRotLerp);
        
    }

    ///bool function checking if enemy should reset their state
    protected bool _hasLostPlayer() {
        bool isLost = false;
        //check if the player has moved out of a specified distance threshold
        //and check if the enemy hasn't been in range for 5f seconds
        if (Time.time > _trackingTimer ||
            playerRangeCheck < Vector3.Distance(transform.position, 
                                                _playerGO.transform.position)) {
            isLost = true;
        }
        return isLost;
    }

    ///function to reset enemy state to searching
    protected void _resetBehaviors() {
        //reset rotation to the stored position
        transform.rotation = _startTransform.rotation;
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
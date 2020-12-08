/*
 * Author: Christian Mullins
 * Summary: First instance of inheritance of the BaseEnemy class.
 */
using UnityEngine;
using System.Collections;

public enum CubeFace {
    PosX, PosY, PosZ, NegX, NegY, NegZ, NULL
}

public class CommonGuard : BaseEnemy {
    ///public
    public EnemyAnimation animationState = EnemyAnimation.Idle;
    [Header("How far away can I see the player?")]
    public float playerRangeCheck = 50.0f;
    public float playerSearchTimer = 15.0f;
    [HideInInspector]
    public Transform lookAtMe;
    ///protected
    protected bool _canSprint = true;
    protected bool _isTrackingPlayer = false;
    protected float _storeRegSpeed;
    protected float _trackingTimer;
    protected EnemyAnimations _animations;
    ///private
    private Vector3 _startLocPos;
    private Quaternion _startQuat;

    ///variables necessary if this instance shoots
    private bool _isShooting = false;
    public GameObject fwdDirGO { get { return _fwdDirGO; } }
    public bool isTrackingPlayer { get { return _isTrackingPlayer; } }

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
            player.GetComponent<PlayerMovement>().playerData.TrackEnemyKills(1, this.gameObject);
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

    //Set to replace the SetActive function as it's hard to call an object that's
    //not active
    public override void activateAI(bool isActive) {
        //execute inheritted version
        base.activateAI(isActive);
        GetComponent<CapsuleCollider>().enabled = isActive;
        if (!isActive) {
            ///baby shark bug fix
            if (_isTrackingPlayer) {
                //are we too close to where the player left?
                if (Vector3.Distance(transform.position, _playerGO.transform.position)
                    < 7.5f) {
                    //reset enemy position completely
                    transform.SetPositionAndRotation(transform.TransformDirection(_startLocPos),
                                                     _startQuat);
                }
                _resetBehaviors();
            }
        }
    }

    /**     PROTECTED FUNCTIONS     */
    protected void Awake() {
        //store startting values
        _startQuat = transform.localRotation;
        _storeRegSpeed = speed;
        _trackingSpeed = _storeRegSpeed;
        _startLocPos = transform.localPosition;
        _animations = GetComponent<EnemyAnimations>();
        //get _lookAtMe reference from player's children
        lookAtMe = GameObject.Find("EnemyLookReference").transform;

        //start scanning for the player
        InvokeRepeating("_scanForPlayer", 0f, Time.fixedDeltaTime);
    }

    protected override void Update() {
        //check if player is found
        if (_isTrackingPlayer) {
            //change behavior
            _myBehavior = Behavior.TrackPlayer;
            _trackingTimer = Time.time + playerSearchTimer;
        }
        if (_hasLostPlayer()) {
            //the player has not be detected and we're not tracking
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
            animationState = EnemyAnimation.Walking;
        }
        else if (_isTrackingPlayer) {
            //override _move() because the enemy will be too focussed on
            //the player to turn around when hitting the wall
            _trackPlayer();
            animationState = EnemyAnimation.Walking;
        }
        else {
            if (_myBehavior == Behavior.Idle) {
                animationState = EnemyAnimation.Idle;
            }
        }
    }

    //LateUpdate() reserved for animation changes
     private void LateUpdate() {
        if (animationState == EnemyAnimation.Idle) {
            _animations.isIdle_CommonGuard();
        }
        if (animationState == EnemyAnimation.Walking) {
            _animations.isWalking_CommonGuard();
        }
        else if (animationState == EnemyAnimation.Shooting) {
            _animations.isShooting_CommonGuard();
        }
    }

    //global vars for this function alone
    private Vector3 _currentLook = Vector3.zero;
    private bool _isScanningRight = false;
    private float _scanTimer = 0.15f;
    //InvokeRepeating that will alter global vars above for tracking
    protected void _scanForPlayer() {
        //initializer
        if (_currentLook == Vector3.zero) {
            //set to left and start scanning right
            _currentLook = transform.TransformDirection(_leftDirGO.transform.localPosition);
            _isScanningRight = true;
        }
        //are we done looking right?
        if (_isScanningRight && _isDirectionCloseEnough(_currentLook, 
                                                        _rightDirGO.transform.localPosition)) {
            _isScanningRight = false;
        }
        //are we done looking left?
        else if (!_isScanningRight && _isDirectionCloseEnough(_currentLook, 
                                                        _leftDirGO.transform.localPosition)) {
            _isScanningRight = true;
        }
        //interpolate directions w/ conditional assignment
        _currentLook = (_isScanningRight) ? 
                       Vector3.Slerp(_currentLook, 
                                     _rightDirGO.transform.localPosition,
                                     _scanTimer) :// * Time.deltaTime) :
                       Vector3.Slerp(_currentLook,
                                     _leftDirGO.transform.localPosition,
                                     _scanTimer);// * Time.deltaTime);
        //Debug.Log("Looking at " + _currentLook);
        //debugging
        //Debug.DrawRay(transform.position, transform.TransformDirection(_currentLook) * 5f, Color.green, 0.75f, true);
        //Debug.DrawRay(transform.position, transform.TransformDirection(-_fwdDirGO.transform.localPosition) * 5f,
        //                Color.green, 0.15f, true);
        //debugging using OnDrawGizmos
        //now raycast, check forward scan and behind
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(_currentLook),
                            out hit, playerRangeCheck)
            || //OR
            Physics.Raycast(transform.position, 
                            transform.TransformDirection(-_fwdDirGO.transform.localPosition), 
                            out hit, 5f)) {
            if (hit.transform.tag == "Player") {
                _isTrackingPlayer = true;
            }
        }
    }

    //protected void OnDrawGizmos() {
        //Gizmos.DrawWireSphere(_fwdDirGO.transform.position, 1.0f);
    //}

    //if the player is never hit, then return Direction.NULL
    [System.Obsolete("Use _scanForPlayer() instead.")]
    protected Direction _isPlayerInRange() {

        Direction dirOfPlayer = Direction.NULL;
        float castDist = playerRangeCheck;
        RaycastHit hit;

        //get location of child GOs
        Vector3 lookForward = _fwdDirGO.transform.position - transform.position;
        Vector3 lookBackward = transform.position - _fwdDirGO.transform.position;
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
        //look behind
        else if (Physics.Raycast(transform.position, lookBackward, out hit, 3.5f)
            && hit.transform.tag == "Player") {
            dirOfPlayer = Direction.Backwards;
        }
        
        ///draw raycast in space to debug
        //draw forward
        //Debug.DrawRay(transform.position, lookForward.normalized * 5f, Color.green, 0.2f, false);
        //Debug.DrawRay(rightHip, lookForward.normalized * 5f, Color.green, 0.2f, false);
        //Debug.DrawRay(leftHip, lookForward.normalized * 5f, Color.green, 0.2f, false);
        //draw left
        //Debug.DrawRay(transform.position, lookLeft.normalized * 5f, Color.green, 0.2f, false);
        //draw right
        //Debug.DrawRay(transform.position, lookRight.normalized * 5f, Color.green, 0.2f, false);
        
        return dirOfPlayer;
    }

    //if the player goes behind a wall and is trying
    protected bool _trackHiddenPlayer() {
        return false;
    }

    //will operate the exact same as the _move function, but won't turn around
    //when facing the wall
    protected void _trackPlayer() {
        //apply rotation to face the player
        Vector3 vecToPlayer = _playerGO.transform.position - transform.position;
        _moveDir = transform.InverseTransformDirection(vecToPlayer).normalized;
        _moveDir.y = 0f;
        float spaceBetween;
#if NOTSHOOTER
        spaceBetween = 0.75f;
#else
        spaceBetween = 1.75f;
        if (TryGetComponent<EnemyShooting>(out EnemyShooting eS)) {
            if (eS.canShootPlayer) {
                //check if the player is in front of me for shooting
                RaycastHit hit;
                //Debug.DrawRay(transform.position, transform.TransformDirection(_fwdDirGO.transform.localPosition * 5f), Color.red, 0.15f);
                if (Physics.Raycast(transform.position, 
                    transform.TransformDirection(_fwdDirGO.transform.localPosition),
                    out hit, playerRangeCheck, 0)) {

                    if (hit.transform.CompareTag("Player")) {
                        eS.shootPlayer();
                        eS.shootTimer = Time.time + eS.shootCoolDown;
                        eS.canShootPlayer = false;
                        //set shooting animation
                        _isShooting = true;
                        animationState = EnemyAnimation.Shooting;
                        Invoke("_stopShootAnimation", 0.6f); //adjust to animation time
                    }
                }
            }
        }
#endif
        if (!_isEnemyFacingWall()) {
            //Enemies that don't shoot will simply ram in to the player
            //distance check to make sure we don't ram the player unless shielded guard
            if (Vector3.Distance(transform.position, _playerGO.transform.position)
                > spaceBetween) {
                //move
                //Vector3 physicsForces = _isClippingWall();
                //if (physicsForces != Vector3.zero) Debug.Log("HIT");
                transform.position += transform.TransformDirection(_moveDir) * speed * Time.fixedDeltaTime;
            }
            //look at the reference point on the player object
            transform.LookAt(lookAtMe, lookAtMe.up);
        }
    }

    //bool function checking if enemy should reset their state
    protected bool _hasLostPlayer() {
        bool isLost = false;
        //check if player has been found in this scene
        if (_playerGO == null) {
            return true;
        }
        //check if the player has moved out of a specified distance threshold
        //and check if the enemy hasn't been in range for 5f seconds
        if (Time.time > _trackingTimer ||
            playerRangeCheck < Vector3.Distance(transform.position, 
                                                _playerGO.transform.position)) {
            isLost = true;
        }
        return isLost;
    }

    //function to reset enemy state to searching
    protected void _resetBehaviors() {
        //change _isTracking bool to reset InvokeRepeating in Update()
        _isTrackingPlayer = false;
        //change behavior
        _myBehavior = Behavior.Idle;
        //reset rotation and position to local
        Vector3 resetPos = new Vector3(transform.localPosition.x,
                                       _startLocPos.y, 
                                       transform.localPosition.z);
        transform.localPosition = resetPos;
        transform.localRotation = _startQuat;
        //reset _moveDir
        Vector3 newMoveDir = (_fwdDirGO.transform.position
                                - transform.position).normalized;
        _moveDir = newMoveDir;
        //invoke scanning 
        InvokeRepeating("_scanForPlayer", 0f, Time.fixedDeltaTime);
    }

    /**
     *  PRIVATE HELPER FUNCTIONS
     */
    
    //Physics replacement
    [System.Obsolete("Discontinued developement.")]
    private Vector3 _isClippingWall() {
        RaycastHit[] hits;
        CapsuleCollider myCap = GetComponent<CapsuleCollider>();
        Vector3 top = transform.localPosition + (myCap.height/2f * Vector3.up);
        Vector3 bottom = transform.localPosition + (myCap.height/2f * Vector3.down);

        hits = Physics.CapsuleCastAll(top, bottom, myCap.radius, Vector3.down);
        foreach (var h in hits) {
            if (h.transform.CompareTag("Wall")) {
               // Debug.Log("point of collision: " + h.point);
                return transform.InverseTransformDirection(h.point);
            }
        }
        //wasn't found
        return Vector3.zero;
    }

    //when using the _scanForPlayer() function, this function will check the current direction
    //vs the desired direction and check if they're close enough
    private bool _isDirectionCloseEnough(Vector3 vA, Vector3 vB) { 
        if (Mathf.Abs(vA.x - vB.x) < 0.01f) {
            if (Mathf.Abs(vA.y - vB.y) < 0.01f) {
                if (Mathf.Abs(vA.z - vB.z) < 0.01f) {
                    return true;
                }
            }
        }
        return false;
    }

    //take fwdDirGO and convert movement to the backwards direction of the enemy
    private Vector3 _getBackDir() {
        Vector3 outputVector = Vector3.zero;
        outputVector = _fwdDirGO.transform.localPosition;

        return outputVector;
    }

    //call using invoke and set time as time to animate
    private void _stopShootAnimation() {
        _isShooting = false;
    }
}
/*
 * Author: Christian Mullins
 * Summary: First instance of inheritance of the BaseEnemy class.
 */
using UnityEngine;

public enum CubeFace {
    PosX, PosY, PosZ, NegX, NegY, NegZ, NULL
}

public class CommonGuard : BaseEnemy {
    ///public
    public EnemyAnimation animationState = EnemyAnimation.Idle;
    [Header("How far away can I see the player?")]
    public float playerRangeCheck = 20.0f;
    public float playerSearchTimer = 15.0f;
    [HideInInspector]
    public Transform lookAtMe;
    ///protected
    protected bool _canSprint = true;
    protected bool _isTrackingPlayer = false;
    protected float _storeRegSpeed;
    protected float _trackingTimer;
    ///private
    private GameObject _leftDirGO, _rightDirGO;
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
        _startQuat = transform.localRotation;
        _storeRegSpeed = speed;
        _trackingSpeed = _storeRegSpeed;
        _startLocPos = transform.localPosition;
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
        //get _lookAtMe reference from player's children
        lookAtMe = GameObject.Find("EnemyLookReference").transform;
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
                    //set shooting animation
                    _isShooting = true;
                    animationState = EnemyAnimation.Shooting;
                    Invoke("_stopShootAnimation", 0.6f); //adjust to animation time
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
        else if (!_canSprint && !_isShooting) {
            //the enemy is currently sprinting and not shooting
            animationState = EnemyAnimation.Running;
        }
        else if (!_isShooting) {
            //default state that shouldn't be interupted by shooting
            animationState = EnemyAnimation.Walking;
        }
    }

    //if the player is never hit, then return Direction.NULL
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
        //Debug.DrawRay(transform.position, lookForward.normalized, Color.black, 0.2f, false);
        //Debug.DrawRay(rightHip, lookForward.normalized, Color.red, 0.2f, false);
        //Debug.DrawRay(leftHip, lookForward.normalized, Color.blue, 0.2f, false);
        //draw left
        //Debug.DrawRay(transform.position, lookLeft.normalized, Color.red, 0.2f, false);
        //draw right
        //Debug.DrawRay(transform.position, lookRight.normalized, Color.blue, 0.2f, false);
        
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
        Vector3 vecToPlayer = (_playerGO.transform.position
                                - transform.position).normalized;
        _moveDir = vecToPlayer;
        _moveDir.y = 0;
        float spaceBetween;
#if NOTSHOOTER
        spaceBetween = 0.75f;
#else
        spaceBetween = 2.5f;
#endif
        //try using only quaternions (NAH)
        if (!_isEnemyFacingWall()) {
            //Enemies that don't shoot will simply ram in to the player
            //raycast to check if I'm too close to the player
            if (Vector3.Distance(transform.position, _playerGO.transform.position)
                < spaceBetween) {
                //move
                transform.localPosition += _moveDir * speed * Time.fixedDeltaTime;
            }
            /**
             * ADDITIONAL FEATURES TO BE ADDED
             */ 
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
        //transform.rotation = _startQuat;
        Vector3 resetPos = new Vector3(transform.localPosition.x,
                                       _startLocPos.y, 
                                       transform.localPosition.z);
        transform.localPosition = resetPos;
        transform.localRotation = _startQuat;
        //reset _moveDir
        Vector3 newMoveDir = (_fwdDirGO.transform.position
                                - transform.position).normalized;
        _moveDir = newMoveDir;
    }

    //call using invoke and set time as time to animate
    private void _stopShootAnimation() {
        _isShooting = false;
    }
}
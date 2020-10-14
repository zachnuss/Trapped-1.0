/*
 * Author: Christian Mullins
 * Summary: First instance of inheritance of the BaseEnemy class.
 */
using UnityEngine;

public class CommonGuard : BaseEnemy {
    //new public values
    public float shootCoolDown = 2f;
    public GameObject projectilePrefab;
    [Header("How far away can I see the player?")]
    public float playerRangeCheck = 10.0f;

    ///protected
    protected bool _canSprint = true;
    protected bool _isTrackingPlayer = false;
    ///private
    private float _storeRegSpeed;
    private float _shootTimer = 0f;
    private GameObject _leftDirGO, _rightDirGO, _fwdDirGO;

    void Awake() {
        //store start speed for sprinting
        _storeRegSpeed = speed;
        //get directional children
        for (int i = 0; i < 3; ++i) {
            GameObject assigningGO = transform.GetChild(i).gameObject;
            //assign gameObject to a child
            if (assigningGO.name == "LeftChild") { 
                _leftDirGO = assigningGO;
            }
            else if (assigningGO.name == "RightChild") {
                _rightDirGO = assigningGO;
            }
            else {
                _fwdDirGO = assigningGO;
            }
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

            //sprint
            if (_canSprint) {
                //increase speed for tracking
                speed = _storeRegSpeed * 1.5f;
                Invoke("_sprintCoolDown", 2.5f); //returns speed back when invoked
                _canSprint = false;
            }
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

        Vector3 lookForward = transform.GetChild(0).position - transform.position;
        Vector3 lookRight = _rightDirGO.transform.position - transform.position;
        Vector3 lookLeft = _leftDirGO.transform.position - transform.position;


        //look forward
        if ((Physics.Raycast(transform.position, lookForward + lookRight, out hit, castDist / 2f)
            || Physics.Raycast(transform.position, lookForward + lookRight, out hit, castDist / 2f)
            || Physics.Raycast(transform.position, lookForward, out hit, castDist))
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
        //draw left
        //Debug.DrawRay(transform.position, lookLeft.normalized, Color.red, 0.2f, false);
        //draw right
        //Debug.DrawRay(transform.position, lookRight.normalized, Color.blue, 0.2f, false);

        return dirOfPlayer;
    }

    //shoot in front of the enemy
    protected void _shootPlayer() {
        //check your timer
        if (Time.time > _shootTimer) {
            //instantiate
            GameObject newBullet;
            Quaternion zerodQ = new Quaternion(0f, 0f, 0f, 1f);
            newBullet = Instantiate(projectilePrefab, _fwdDirGO.transform.position,
                                    zerodQ, null) as GameObject;
            //pass bullet speed value as 

            //move


            //set a new timer
            _shootTimer = Time.time + shootCoolDown;
        }
    }

    //call as invoke so the 
    protected void _sprintCoolDown() {
        speed = _storeRegSpeed;
        _canSprint = true;
    }


    ///private helper functions
    private GameObject _getDirChild(Direction dir) {
        GameObject dirChild;
        switch (dir) {
            case Direction.Forward:
                dirChild = _fwdDirGO;
                break;
            case Direction.Right:
                dirChild = _rightDirGO;
                break;
            case Direction.Left:
                dirChild = _leftDirGO;
                break;
            default:
                dirChild = null;
                break;
        }
        return dirChild;
    }
}
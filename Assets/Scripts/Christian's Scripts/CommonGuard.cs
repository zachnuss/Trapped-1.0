/*
 * Author: Christian Mullins
 * Summary: First instance of inheritance of the BaseEnemy class.
 */
using UnityEngine;

public class CommonGuard : BaseEnemy {
    //new public values
    //public float shootCoolDown = 2f;
    //public GameObject projectilePrefab;
    [Header("How far away can I see the player?")]
    public float playerRangeCheck = 10.0f;

    ///protected
    protected bool _canSprint = true;
    protected bool _isTrackingPlayer = false;
    //protected bool _canShootPlayer = false;
    protected float _storeRegSpeed;
    ///private
    //private float _shootTimer = 0f;
    private GameObject _leftDirGO, _rightDirGO, _fwdDirGO;

    ///variables necessary if this instance shoots
    private bool _doesEnemyShoot = false;
    public GameObject fwdDirGo { get { return _fwdDirGO; } }
    private EnemyShooting _shooterScript;

    protected void Awake() {
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

        /**     Check if this instance shoots   **/
        if (TryGetComponent<EnemyShooting>(out EnemyShooting myES)) {
            _shooterScript = myES;
            _doesEnemyShoot = true;
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
            if (_doesEnemyShoot) {
                //this code will be unreachable if EnemyShooting is not attached
                //to this GameObject
                if (_shooterScript.canShootPlayer 
                    && dirOfPlayer == Direction.Forward) {
                    _shooterScript.shootPlayer();
                    _shooterScript.shootTimer = Time.time + _shooterScript.shootCoolDown;
                    _shooterScript.canShootPlayer = false;
                }
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



}
/*
 * Author: Christian Mullins
 * Summary: Parent class for all enemies in the game.
 */
using UnityEngine;

///Enemy enums
public enum Behavior {
    Idle, ChangeDirection, GoForward, TrackPlayer, AttackPlayer
}

public enum Direction {
    Forward, Right, Left, Backwards, NULL
}

public enum EnemyAnimation {
    Idle, Walking, Running, Shooting
}

public class BaseEnemy : MonoBehaviour {
    /*
     *  REQUIRED VALUES AND FUNCTIONS SPECIFIED ON TRELLO:
     *      Health
     *      Damage
     *      Speed
     *      pointValue
     *      levelup()
     *      onDeath() //gives points to player
     *      takeDamage(int attackDamage) //function for enemy to call to do 
     *                                      attack damage to player
     */
    /**
     * CLASS VARIABLES
     */

    //public (variables below will be set in the inspector)
    public int health;
    public int damage;
    public float speed;
    public int pointValue;
    [Range(1f, 5f)] public float rateOfBehaviorChange = 2f;
    public GameObject specialCoin;
    public CubemapFace myFaceLocation { get { return _myFaceLocation; } }
    //protected
    protected Behavior _myBehavior;
    protected float _trackingSpeed;
    protected Vector3 _moveDir; //movement
    protected GameObject _fwdDirGO, _leftDirGO, _rightDirGO;
    protected GameObject _playerGO;
    protected EnemyAnimations _enemyAnim;
    //private
    private Vector3 _rotVal; //rotation
    private float _wallDetectRay = 0.75f;
    private bool _hasHitWall = false;
    private CubemapFace _myFaceLocation;

    [Header("Modifers")]
    public bool doubleDamageMod = false;
    public bool sheildMod = false;
    //increase speed
    public bool advancedStimsMod = false;
    bool damageStbTimer = false;
    float damageTimer = 0f;
    public GameObject sheildObj;
    //for double damage in enemy script
    
    LevelSetup _lvlSetUp;

    /**
     * CLASS FUNCTIONS
     */
    ///public
    public virtual void takeDamage(GameObject player) {
        //take health away
        health -= _playerGO.GetComponent<PlayerMovement>().damage;
        //did the enemy die?
        if (health < 1) {
            health = 0;
            //remove from enemy listener
            EnemyListener.Instance.deleteFromList(this);
            //give score to player
            player.GetComponent<PlayerMovement>().playerData.AddScore(pointValue);
            player.GetComponent<PlayerMovement>().playerData.TrackEnemyScore(pointValue);
            player.GetComponent<PlayerMovement>().playerData.TrackEnemyKills(1, this.gameObject);
            if (Random.Range(0f, 100f) <= 30)
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
    
    //level entered for parameter must be index (i.e. level 1 = 0)
    public void levelUp(int level) {
        float multiplier = 1.1f;
        float speedMultipler = 1.025f; //set lower so keep better flow

        multiplier = Mathf.Pow(multiplier, level);
        speedMultipler = Mathf.Pow(speedMultipler, level);
        //raise stats
        health = Mathf.FloorToInt(health * multiplier);
        damage = Mathf.FloorToInt(damage * multiplier);
        speed = Mathf.FloorToInt(speed * speedMultipler);
        pointValue = Mathf.FloorToInt(pointValue * multiplier);
    }

    //Set to replace the SetActive function as it's hard to call an object that's
    //not active
    public virtual void activateAI(bool isActive) {
        //HallwayBot(Rumba) instance
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i) {
            GameObject curGO = transform.GetChild(i).gameObject;
            if (curGO.tag != "ShieldMod") {
                curGO.SetActive(isActive);
            }
        }
        //set behavior
        if (isActive) {
            InvokeRepeating("_changeBehavior", 0f, rateOfBehaviorChange);
        }
        else {
            CancelInvoke("_changeBehavior");
            _myBehavior = Behavior.Idle;
        }
        GetComponent<AudioSource>().enabled = isActive;
    }

    ///protected
    protected void _changeBehavior() {
        //transfer rate of behavior change and get wait time
        float randWaitTime = Random.Range(0f, rateOfBehaviorChange);

        switch (_myBehavior) {
            case Behavior.Idle:
                //increase randWaitTime randomly
                if (Random.Range(0, 3) == 0) {
                    randWaitTime /= 2f;
                    //randWaitTime += 0.75f; // try to edit using division
                }
                //enemy will remain in position (see Update())
                break;
            case Behavior.ChangeDirection:
                //get random direction to move
                Direction randDir = _goLeftOrRightDirection();
                _turnThisDirection(randDir);
                break;
            case Behavior.GoForward:
                //don't change action in any way
                break;
            case Behavior.AttackPlayer:
                //suspend wait time, focus on tracking the player
                break;
            case Behavior.TrackPlayer:
                //suspend wait time, focus on tracking the player
                break;
            default: break;
        }
        //randomly change behavior the next time this is called
        _myBehavior = (Behavior)Random.Range(0, 3); //max limit is exclusive
    }

    //move the player in the direction specified
    protected void _move(Vector3 moveDir) {
        //immediately return if zeroed values
        if (moveDir == Vector3.zero) return;
        //change direction if I'm gonna hit the wall
        if (_isEnemyFacingWall()) {
            Direction outputDir = Direction.Backwards;
            //get rand dir if I've already hit a wall
            if (_hasHitWall) { 
                outputDir = _goLeftOrRightDirection();
            }

            _turnThisDirection(outputDir);
            //swap bool
            _hasHitWall = (_hasHitWall) ? false : true;
        }

        //finally move
        transform.position += moveDir * speed * Time.deltaTime;
    }

    //take a direction, based on Direction enum
    protected void _turnThisDirection(Direction dir) {
        //the type of axis turn will be determined by CubemapFace
        float axisTurn = 0f;
        //using myFace for CubemapFace, alter stuff
        switch (dir) {
            case Direction.Right:
                //oneEnemyPrint("CommonGuard", "turning Right");
                axisTurn = 90f;
                break;
            case Direction.Left:
                //oneEnemyPrint("CommonGuard", "turning Left");
                axisTurn = -90f;
                break;
            case Direction.Backwards:
                //return positive or negative 180f
                //oneEnemyPrint("CommonGuard", "turning Backwards");
                float posOrNeg = (Random.Range(0, 2) == 0) ? 180 : -180;
                axisTurn = posOrNeg;
                break;
            default: //if forward or null, don't do anything
                break;
        }
        //turn only if there's a value change
        if (axisTurn != 0f) {
            //get appropriate turning axis based on CubemapFace
            Vector3 turningVector = Vector3.zero;
            //PositiveY  is
            turningVector.y += axisTurn;
            //transform.Rotate(turningVector); //mess with local vs world space
            transform.localEulerAngles += turningVector;
            //change facing dir to match rotation
            Vector3 newMoveDir = Vector3.MoveTowards(_fwdDirGO.transform.position,
                                                     transform.position,
                                                     Time.maximumDeltaTime);
            newMoveDir.y = 0; //restrict y-axis movement
            _moveDir = (_fwdDirGO.transform.position
                        - transform.position).normalized;
        }
    }

    protected bool _isEnemyFacingWall() {
        //local vars
        bool isFacingWall = false;
        RaycastHit hit;
        //check what's in fron using Raycast
        //get approximate width of the player
        /*
        Vector3 rightHip = (_rightDirGO != null) ? (_rightDirGO.transform.position 
                                                    + transform.position) / 2f
                                                 : Vector3.zero;
        Vector3 leftHip = (_leftDirGO != null) ? (_leftDirGO.transform.position
                                                    + transform.position) / 2f
                                                 : Vector3.zero;
        */
        //if (rightHip != null && leftHip != null) {
        //    Debug.DrawRay(transform.position, _moveDir * 2f, Color.red);
        //    Debug.DrawRay(rightHip, _moveDir * 2f, Color.green);
        //    Debug.DrawRay(leftHip, _moveDir * 2f, Color.green);
        //}
        if (Physics.SphereCast(transform.position, 0.25f, _moveDir, out hit, _wallDetectRay)) {
           // || Physics.Raycast(rightHip, _moveDir, out hit, _wallDetectRay)
           // || Physics.Raycast(leftHip, _moveDir, out hit, _wallDetectRay)) {
           //don't change direction if I'm looking at the player
           if (hit.transform.tag == "Wall") {
                isFacingWall = true;
           }
           //am I hitting myself?
           else if (hit.transform.name == _fwdDirGO.name) {
                Debug.LogWarning("BaseEnemy: hitting child for raycast"); 
           }
        }
        return isFacingWall;
    }

    protected virtual void Update() {
        if (_myBehavior != Behavior.Idle) {
            //move enemy
            _move(_moveDir);
        }

        //sheild mod
        DamageStandbyTimer();
    }

    //this function will act like onDeath (doesn't need to be called manually)
    protected virtual void OnDestroy() {
        //call singleton to add score to game

        ///collectable
        ///STAND IN CODE FOR THE FUTURE
        float dropChance = 0.3f;
        GameObject droppedCol = null; //= getCollectable(); //<-- hypothetical function
        if (Random.Range(0.0f, 1.0f) <= dropChance) {
            //make sure this doesn't parent to the enemy on Instantiation
            //(otherwise it will be destroyed immediately)
            //Instantiate(droppedCol, transform, true);
        }
    }

    ///private
    /*
     *  AWAKE() RESERVED FOR CHILDREN
     */

    private void Start() {
        //initialize variables
        _playerGO = GameObject.FindWithTag("Player");
        //uncommenting below once work on animations begin
        //_enemyAnim = GetComponent<EnemyAnimations>();

        //get where I'm facing for initial variables
        int childrenNum = transform.childCount;
        for (int i = 0; i < childrenNum; ++i) {
            if (transform.GetChild(i).name == "FrontChild") {
                _fwdDirGO = transform.GetChild(i).gameObject;
            }
            if (transform.GetChild(i).name == "LeftChild") { 
                _leftDirGO = transform.GetChild(i).gameObject;
            }
            if (transform.GetChild(i).name == "RightChild") {
                _rightDirGO = transform.GetChild(i).gameObject;
            }
        }

        Vector3 initialDir = _fwdDirGO.transform.position - transform.position;
        _moveDir = initialDir.normalized;

        //get my level based on index (i.e. level 1 = 0)
        if (_playerGO != null) {
            int curLevelIndex = _playerGO.GetComponent<PlayerMovement>().playerData.OnLevel;
            //level up based on level index
            levelUp(curLevelIndex);
        }

        //loop to change behavior sporatically
        InvokeRepeating("_changeBehavior", 0.5f, rateOfBehaviorChange);


        //get mods from level obj
        //if statement to prevent error with CommonGuard in MainMenu scene
        //check my current cubeface
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0) {
            SetModifiers();
            _myFaceLocation = CubemapFace.Unknown;
            string parentName = transform.parent.name;
            switch (parentName)
            {
                /** x axis and z axis are swapped in scene!! **/
                case "Z": //x-axis
                    _myFaceLocation = (transform.position[0] > 0f) ? CubemapFace.PositiveX
                                                                   : CubemapFace.NegativeX;
                    break;
                case "Y": //y-axis
                    _myFaceLocation = (transform.position[1] > 0f) ? CubemapFace.PositiveY
                                                                   : CubemapFace.NegativeY;
                    break;
                case "X": //z-axis
                    _myFaceLocation = (transform.position[2] > 0f) ? CubemapFace.PositiveZ
                                                                   : CubemapFace.NegativeZ;
                    break;
            }
        }
    }

    //get random int to cast to Direction enum
    private Direction _goLeftOrRightDirection() {
        int randInt = Random.Range(1, 3);//left or right only
        return (Direction)randInt;
    }

    private void OnTriggerEnter(Collider other) {
        //if the enemy is going to collide with a door, turn around
        //this should solve potential bugs in the future
        if (other.transform.tag == "Door") {
            //turn around if the enemy's about to hit the door
            _turnThisDirection(Direction.Backwards);
        }

        ///enemy will take damage from the player through the ProjectileScript
    }


    ///DEBUGGING FUNCTION TO ONLY CALL FROM A SINGLE ENEMY
    private void oneEnemyPrint(string enemyName, string printing) {
        if (name == enemyName) {
            Debug.Log(enemyName + ": " + printing);
        }
    }


    // Enemy Modifiers - Added By Dylan
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// - if enemy takes damage, begin timer, once timer is reached sheild can regen health
    /// - will run in update, bool is active when takes damage, turns off when timer is done
    /// </summary>
    void DamageStandbyTimer()
    {
        if (damageStbTimer)
        {
            damageTimer += Mathf.RoundToInt(Time.deltaTime);
            if (damageTimer >= 5)
            {
                damageStbTimer = false;
                damageTimer = 0;
                sheildObj.GetComponent<ForceFieldsEnemy>().ableToRecharge = true;
            }
        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// this runs when enemy takes damage from player
    /// </summary>
    public void SheildRegenStop()
    {
        damageStbTimer = true;
        sheildObj.GetComponent<ForceFieldsEnemy>().ableToRecharge = false;
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-26-2020
    /// 
    /// initial setup of modifiers
    /// </summary>
    void SetModifiers()
    {
        _lvlSetUp = GameObject.Find("LevelSetup").GetComponent<LevelSetup>();
        for (int modIndex = 0; modIndex < _lvlSetUp.currentModsInLevel.Length; modIndex++)
        {
            if (_lvlSetUp.currentModsInLevel[modIndex].modType == modifierType.doubleDamageMOD && _lvlSetUp.currentModsInLevel[modIndex].modActive)
            {
                doubleDamageMod = true;
            }
            if(_lvlSetUp.currentModsInLevel[modIndex].modType == modifierType.shields_and_regainMOD && _lvlSetUp.currentModsInLevel[modIndex].modActive)
            {
                sheildMod = true;
                sheildObj.SetActive(true);
            }
            if(_lvlSetUp.currentModsInLevel[modIndex].modType == modifierType.AdvancedSimulant && _lvlSetUp.currentModsInLevel[modIndex].modActive)
            {
                advancedStimsMod = true;
                Debug.Log("OG SPEED: " + speed);
                //increase to speed by 25%
                speed = speed + (speed * 0.25f);
                Debug.Log("New speed yo: " + speed);
            }
        }
        if (!sheildMod)
            sheildObj.SetActive(false);
    }
}

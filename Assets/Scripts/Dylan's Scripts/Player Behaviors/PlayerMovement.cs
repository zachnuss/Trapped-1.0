using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;


/// <summary>
/// Dylan Loe
/// 
/// Basic movement system
/// 
/// Notes:
/// - Player rotates on local axis so it depends on which way player is rotated
/// - Will incorate a shoot function
/// - Movement is based on joystick orientation
/// - shooting i buttom button
/// </summary>


public class PlayerMovement : MonoBehaviour
{
    //animation states for player
    //top
    public enum playerTopState
    {
        idle,
        moving,
        firing,
        interacting
    };
    //legs
    public enum playerBottomState
    {
        idle,
        walking
    }


    public UIInGame gamerUI;
    //set up for rotation and new rotation orientation
    [Header("Parent object of this player obj")]
    public GameObject parent;
    public GameObject follower;
    private GameObject[] character = new GameObject[8];
    //new rotation orientation player moves to
    //Quaternion targetRotation;
    //PlayerInputActions controls;
    //Vector3 _playerAngle;
    [Header("Player movement speed")]
    public float movementSpeed = 1.0f;
    Vector2 movementInput;
    //rotation
    [Header("Rotation Speed")]
    public float turnSpeed = 20f;
    public float lookSpeed = 30f;
    float _angle;
    //player looking rotation
    Vector2 lookingInput;

    [Header("Current Player Stats - Set on Scene Start")]
    public float health;
    public int damage;
    public float speedMultiplier;

    [Header("Player Animation State")]
    public playerBottomState animBottomState;
    public playerTopState animTopState;
    playerTopState _localTopState = playerTopState.idle;
    public int bulletDamage;
    //Camera
    // public CamLookAt playerCam;
    //level setup script

    //when we have successfully rotated
    [Header("Shows if player is off the edge")]
    public bool overTheEdge = false;
    bool onDoor = false;

    //when we hit a door the player rotates and moves to this transform taken from the door prefab
    private Transform _rotationTrans;

    /// <summary>
    /// Interpolation with sin easing, most of these will be private
    /// may use c2 and c3 for bezier or multiple interpolation points in future
    /// </summary>
    private Transform c0, c1;
    private Quaternion r01;
    private float timeDuration = 1.2f;
    public bool checkToCalculate = false;
    private Vector3 p01;

    //for smooth parent rotation
    private Quaternion par01;
    private Transform pc1, pc0;

    public EasingType easingTypeC = EasingType.linear;
    public bool moving = false;
    private float timeStart;
    private float u, u2;
    float easingMod = 2f;

    //Shoot Code Variable
    [Header("Player Bullet Var")]
    public GameObject Player_Bullet; //Bullet prefab

    //player data scriptable obj
    [Header("Player Data")]
    public PlayerData playerData;

    /// <summary>
    /// Scene Transition and endgame
    /// 
    /// </summary>
    private GameObject teleporterTracker;//Assign before load, set to private if unneeded
   // public string nextScene; //Target Level
    public Animator[] transition; //Transition animator
    public float transitionTime = 1;
    private int rng;
    public bool firingState;
    bool _repeatedFire;
    //set to time to run full animation before repeat, may be a bit shorter so it works better
    float fireAnimationTime = 1f;
    public float localTimer;

    [Header("Player Modifiers")]
    //player choosen mods
    public bool healthRegen = false;
    public bool doubleScoreMod = false;
    
    //displays timer per level (resets at level start and ends at level end
    [Header("UI")]
    public Text timerText;

    [Header("Player Animators")]
    public Animator top;
    private GameObject topObj;
    //public Animator legs;
    public PlayerAnimations playerAnimations;

    //public GameObject top;
    public AnimatorStateInfo stateInfo;
    public float shootingAnimLength = 1.5f;

    bool _fireCoolDown;
    [Header("Fire Rate")]
    public float fireRate = 0.2f;
    

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < character.Length; i++)
        {
            character[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
        }

        //SetPlayerModifiers(); Player Mods set up in LevelSetup obj and script
        SetPlayerStats();

        teleporterTracker = GameObject.FindGameObjectWithTag("GoalCheck"); //assumes we check on construction of the player, with a new player every level - Wesley
        rng = Random.Range(0, transition.Length);
        localTimer = playerData.timerBetweenLevels;
        InvokeRepeating("PerSecond", 0f, 1f); //Every second, give score equal to 1*the level count. - Wesley
    }


    // Used for physics 
    private void FixedUpdate()
    {
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //cant move if we are rotating
        if (!overTheEdge && !moving)
        {
            Movement();
        }

        Timer();

        //moves player to next side of cube
        if (_rotationTrans != null)
        {
            Interpolation();
        }
    }

    float firingTimer = 0;

    // used for updating values and variables
    private void Update()
    {
        //movement
        //detects if player is over an edge
        if (DetectEdge())
        {
            overTheEdge = true;
        }
        
        //Interpolation stuff, for rotation onto next side
        if (overTheEdge && onDoor && !checkToCalculate && !moving)
        {
            //Debug.Log("got trans, start interpolation");
            //if we hit the door and are off the cube
            checkToCalculate = true;
        }

        //temp commented out
        if(firingState)
        {
            firingTimer += Time.deltaTime;
            if(firingTimer >= shootingAnimLength)
            {
                firingState = false;
                firingTimer = 0;
               // Debug.Log(stateInfo.length);
            }
        }

       // SetAnimation();
       if(_localTopState != animTopState)
        {
            _localTopState = animTopState;
            SetAnimation();
        }
    }

    //moves player based on equation
    //may need to update to bezier for smoother rotation
    void Interpolation()
    {
        if (checkToCalculate)
        {
            c0 = this.transform;
            c1 = _rotationTrans;

            //smooth parent movement (for camera)
            pc0 = parent.transform;
            pc1 = _rotationTrans;
            
            checkToCalculate = false;
            moving = true;
            timeStart = Time.time;
            OnPlayerRotation();
        }

        if (moving)
        {
            u = (Time.time - timeStart) / timeDuration;

            if (u >= 1)
            {
                //when we reach new pos
                parent.transform.rotation = _rotationTrans.transform.rotation;
              
                u = 1;
                moving = false;
                _rotationTrans = null;
                overTheEdge = false;
            }

            //adjsut u value to the ranger from uMin to uMax
            //different types of eases to avoid snaps and rigidness
            u2 = EaseU(u2, easingTypeC, easingMod);
            //interpolation equation (with min and max)

            //standard linear inter
            //position
            p01 = (1 - u) * c0.position + u * c1.position;

            //quaternions are different
            //use unities built in spherical linear interpolation
            //SLERP
            r01 = Quaternion.Slerp(c0.rotation, c1.rotation, u);

            par01 = Quaternion.Slerp(pc0.rotation, pc1.rotation, u2);

            //apply those new values
            transform.position = p01;
            transform.rotation = r01;
            follower.transform.rotation = r01;
            parent.transform.rotation = par01;
        }
    }
   
    /*
     * Movement and Player Looking Rotation
     * Uses player controller and both joysticks for movement and for player sights
     */
    //for movement
    private void OnMove1(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
    //for looking
    private void OnLook(InputValue value)
    {
        lookingInput = value.Get<Vector2>();
        Debug.Log(lookingInput);
    }
    //joysticks
    public float JoystickH()
    {
        //float r = 0.0f;
        float h = movementInput.x;
        //r += Mathf.Round(h);
        return Mathf.Clamp(h, -1.0f, 1.0f);
    }
    public float JoystickV()
    {
        //float r = 0.0f;
        float v = movementInput.y;
        //r += Mathf.Round(v);
        return Mathf.Clamp(v, -1.0f, 1.0f);
    }
    public Vector3 MainJoystick()
    {
        return new Vector3(JoystickH(), 0, JoystickV());
    }
    void RotateMovement(Vector3 movement)
    {
        //convert joystick movements to angles that we can apply to player rotation
        _angle = Mathf.Atan2(movement.x, movement.z);
        _angle = Mathf.Rad2Deg * _angle;

        //local angles are used since its a child, the player parent is set to keep track of the global rotation
        transform.localRotation = Quaternion.Euler(0 , Mathf.LerpAngle(transform.localEulerAngles.y, _angle, Time.deltaTime * turnSpeed), 0 ); //transform.localEulerAngles.x 

        //base movement is just 1.0
        float boost = movementSpeed * speedMultiplier;
        float newSpeed = movementSpeed + boost;

        //player is always moving forward, player is just adjsuting which way they move forward (always local forward so we can have player move consistentaly forward on each side)
        transform.position += transform.forward * newSpeed *speedAdjustment()* Time.deltaTime;
    }

    void Movement()
    {
        Vector3 movement = MainJoystick();
        //Debug.Log(movement);
        //only move if player gives input
        if (movement != Vector3.zero)
        {
            RotateMovement(movement);
            animBottomState = playerBottomState.walking;
            //turned off temp cause we dont have animations lol
            if(!firingState)
                animTopState = playerTopState.moving;
        }
        else
        {
            animBottomState = playerBottomState.idle;
            if (!firingState)
                animTopState = playerTopState.idle;
        }
    }
    //make movement not constant
    public float speedAdjustment()
    {
        float speedMod = Mathf.Sqrt((movementInput.x * movementInput.x) + (movementInput.y * movementInput.y));

        //player legs walking speed is determined here
        //SET ANIMATION SPEED HERE (multiply anim speed by speed mod)

        return speedMod;
    }

    //looking rotation (Not running yet)
    public float JoystickLookingH()
    {
        //float r = 0.0f;
        float h = movementInput.x;
        //r += Mathf.Round(h);
        return Mathf.Clamp(h, -1.0f, 1.0f);
    }
    public float JoystickLookingV()
    {
        //float r = 0.0f;
        float v = movementInput.y;
        //r += Mathf.Round(v);
        return Mathf.Clamp(v, -1.0f, 1.0f);
    }
    public Vector3 LookJoystick()
    {
        return new Vector3(JoystickLookingH(), 0, JoystickLookingV());
    }
    void Looking()
    {
        Vector3 looking = LookJoystick();
        //Debug.Log(movement);
        //only move if player gives input
        if (looking != Vector3.zero)
        {
            LookMovement(looking);
            //animTopState = playerTopState.moving;
        }
       // else
          //  animTopState = playerTopState.idle;
    }

    void LookMovement(Vector3 movement)
    {
        //convert joystick movements to angles that we can apply to player rotation
        _angle = Mathf.Atan2(movement.x, movement.z);
        _angle = Mathf.Rad2Deg * _angle;

        //local angles are used since its a child, the player parent is set to keep track of the global rotation
        //rotates top half with the gun
        if(topObj != null)
            topObj.transform.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(transform.localEulerAngles.y, _angle, Time.deltaTime * lookSpeed), 0);

        
    }

    //once player reaches new side
    void OnPlayerRotation()
    {
        //runs when player moves to next cube (runs only once)
        //camera rotation
        Transform newCameraTrans = _rotationTrans;
    }

    //when player gets to edge
    bool DetectEdge()
    {
        bool noFloor = false;
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down), Color.green);

        int layerMask = 1 << 8;
        //everything but 8
        layerMask = ~layerMask;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) * 6, out hit, 1, layerMask))
        {
            //if we dont hit anything, char is hanging over edge
            //if(hit.collider.tag != "Cube")
            noFloor = false;
        }
        else
            noFloor = true;

        return noFloor;
    }

    //when player hits the attack button
    void OnAttack()
    {
        if (!_fireCoolDown)
        {
            //runs everytime our char attacks
            //Wesley-Code
            GameObject bullet = Object.Instantiate(Player_Bullet, transform.position, transform.rotation);
            //ZACHARY ADDED THIS
            StartCoroutine(bullet.GetComponent<ProjectileScript>().destroyProjectile());
            //just to destroy stray bullets if they escape the walls
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(bullet.transform.forward * 1000);
            // _repeatedFire = true;
            //when we fire we run fire animation once (firing state is active while animation is active)
            // StartCoroutine(FireState());

            //start animation
            firingState = true;
            //Debug.Log("fire");
            //temporary turned off cause i dont have animations (will ignore firing for now)
            animTopState = playerTopState.firing;
            playerAnimations.isFiringTop();
            firingTimer = 0;
            StartCoroutine(FireCoolDown());
        }
    }
    
    //triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Door")
        {
            onDoor = true;
            _rotationTrans = other.gameObject.GetComponent<DoorTrigger>().moveLocation;
            //c2 = other.gameObject.GetComponent<DoorTrigger>().moveMid;
            //other.gameObject.GetComponent<DoorTrigger>().SwitchDirection();

            //checkToCalculate = true;
        }

        //end game
        if (other.tag == "Goal")
        {
            //Debug.Log("goal");
            if(!other.GetComponent<TeleBool>().active)
                   gamerUI.UpdateObjText();
            //Debug.Log("hit");
            other.GetComponent<TeleBool>().active = true;
            //instead of destroying io made the game object change color so we dont get an error when we have multiple keys
            other.GetComponent<TeleBool>().onPress();
            if (teleporterTracker.GetComponent<TeleporterScript>().GoalCheck(teleporterTracker.GetComponent<TeleporterScript>().teleporters))
            {
                playerData.localHealth = health;
                StartCoroutine(LoadTargetLevel());
            }
            //Destroy(other.gameObject);
        }

        //adding christans damage code
        if (other.gameObject.tag == "Bullet")
        {
            //destroy object
            Destroy(other.transform.gameObject);
            //decrement health
            takeDamage(bulletDamage); //Olivia changed this
            Debug.Log("Current health: " + health);
        }

        ///added by Christian to take damage when colliding with an enemy
        if (other.gameObject.tag == "Enemy")
        {
            //in the future damage will need to be derived specifically from the enemy type
            takeDamage(other.GetComponent<BaseEnemy>().damage);

            //could apply a random percentage of extra damage for future iterations
            if(other.gameObject.GetComponent<BaseEnemy>().doubleDamageMod)
                takeDamage(other.GetComponent<BaseEnemy>().damage);

            //Debug.Log("Current health: " + health);
        }

        if(other.gameObject.tag == "PowerUp")
        {
            //Debug.Log("Hit powerup");
            PickedPowerUp(other.gameObject.GetComponent<PowerUpDrop>().type, other.gameObject.GetComponent<PowerUpDrop>().timer, other.gameObject.GetComponent<PowerUpDrop>().powerUpDuration);
            //run animation on powerup (if any)

            Destroy(other.gameObject);
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Door")
        {
            onDoor = false;
        }
    }

    //easing types
    public float EaseU(float u, EasingType eType, float eMod)
    {
        float u2 = u;
        switch (eType)
        {
            case EasingType.linear:
                u2 = u;
                break;
            case EasingType.easeIn:
                u2 = Mathf.Pow(u, eMod);
                break;
            case EasingType.easeOut:
                u2 = 1 - Mathf.Pow(1 - u, eMod);
                break;
            case EasingType.easeInOut:
                if (u <= 0.5f)
                {
                    u2 = 0.5f * Mathf.Pow(u * 2, eMod);
                }
                else
                {
                    u2 = 0.5f + 0.5f * (1 - Mathf.Pow(1 - (2 * (u - 0.5f)), eMod));
                }
                break;
            case EasingType.sin:
                u2 = u + eMod * Mathf.Sin(2 * Mathf.PI * u);
                break;
            case EasingType.sinIn:
                u2 = 1 - Mathf.Cos(u * Mathf.PI * 0.5f);
                break;
            case EasingType.sinOut:
                u2 = Mathf.Sin(u * Mathf.PI * 0.5f);
                break;
            default:
                break;
        }

        return (u2);
    }

    //setting player stats
    void SetPlayerStats()
    {
        
        health = playerData.localHealth;

        damage = playerData.totalDamageBase + playerData.damageUpgrade;

        gamerUI.healthBarStatus(health);

        speedMultiplier = (playerData.speedUpgrade)/10;
        //Debug.Log(speedMultiplier);

        //Wesley
        ChangeColor(); //Sets in scene start
    }

    //Wesley - Setting player color
    public void ChangeColor()
    {
        for (int i = 0; i < character.Length; i++)
        {
            character[i].GetComponent<SkinnedMeshRenderer>().material = playerData.playerColor[playerData.materialChoice];
        }
    }

    //player takes damage
    public void takeDamage(int damageTaken)
    {
        //damage player
        health -= damageTaken;
        //playerData.localHealth -= damageTaken;
        if (health < 1)
        {
            health = 0; //because negative health looks bad
            //send to GameOver Screen
            Debug.Log("GAME OVER");
            //call SceneManager to get the GameOverScene
            //int gameOverInt = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1;

            //Set Highscore
            playerData.EndGameScoring();

            UnityEngine.SceneManagement.SceneManager.LoadScene(6);
            //DontDestroyOnLoad(GameObject.Find("ScriptManager"));
        }
    }

    //Scene Transitions
    IEnumerator LoadTargetLevel()
    {
        transition[rng].SetTrigger("Start"); //start animation

        yield return new WaitForSeconds(transitionTime); //Time given for transition animation to play

        //SceneManager.LoadScene(nextScene); //Loads target scene
        playerData.BeatLevel();
    }

    //Score - Wesley
    void PerSecond()
    {
        if (!doubleScoreMod)
        {
            playerData.AddScore(1 * (playerData.OnLevel + 1)); //because onlevel is 0 indexed, add 1. //add to total score
            playerData.TrackTimeScore(1 * (playerData.OnLevel + 1)); //add to time only score
        }
        else
        {
            playerData.AddScore(2 * (playerData.OnLevel + 1));
            playerData.TrackTimeScore(2 * (playerData.OnLevel + 1));
        }

        //other things it should do every second

        //health regen  (if active)
        if (healthRegen)
        {
            if (health < playerData.totalHealthBase)
            {
                health += 0.1f;
                gamerUI.healthBarStatus((int)health);
                Debug.Log("regen");
            }
        }
    }

    //UI and TIMER
    void Timer()
    {
        localTimer += Time.deltaTime;
        playerData.totalTime+= Time.deltaTime;
        // Debug.Log("click");
        playerData.timerSec = Mathf.RoundToInt(localTimer);
        if (playerData.timerSec >= 60)
        {
            playerData.timerMin++;
            playerData.timerSec = 0;
            localTimer = 0;

        }

        playerData.UpdateTime();
      //  Debug.Log(_timer);
        DisplayTime();

        
    }
    //update and display timer 
    void DisplayTime()
    {
        //update text here with info from playerdata
       // const string Format = "{22:11:00}";
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", playerData.timerHour, playerData.timerMin, playerData.timerSec);

    }
    //timer counter
    IEnumerator timerCount()
    {
        yield return new WaitForSeconds(1.0f);
        localTimer++; //= Time.deltaTime;
        //Debug.Log("click");

        playerData.UpdateTime();

        DisplayTime();
        StartCoroutine(timerCount());
    }

    //powerup
    void PickedPowerUp(powerUpType type, bool timer, int duration)
    {
        if(timer)
        {
            StartCoroutine(PowerUpDuration(type, duration));
        }
    }
    //how long the powerups last
    IEnumerator PowerUpDuration(powerUpType type, int duration)
    {
        //turn on
        switch (type)
        {
            case powerUpType.damage:
                damage += 5;
                break;
            case powerUpType.speed:
                speedMultiplier += 0.5f;
               // Debug.Log(speedMultiplier);
                break;
            case powerUpType.health:
                if (health < playerData.totalHealthBase)
                {
                    health += 20;
                    playerData.localHealth = health;
                }
                if (health > playerData.totalHealthBase)
                    health = playerData.totalHealthBase;
                gamerUI.healthBarStatus(health);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(duration);
        //turn off
        switch (type)
        {
            case powerUpType.damage:
                damage -= 5;
                break;
            case powerUpType.speed:
                speedMultiplier -= 0.5f;
                break;
            default:
                break;
        }
        //Debug.Log("off");
        //Debug.Log(speedMultiplier);
    }

    //will be in update
    void SetAnimation()
    {
        //playerAnimations.isIdlingTop();
        //based on status enum
        switch (animBottomState)
        {
            case playerBottomState.idle:
                //playerAnimations.isIdlingTop();
                break;
            case playerBottomState.walking:
                //playerAnimations.isIdlingTop();
                break;
            default:
                break;
        }

        switch (animTopState)
        {
            case playerTopState.idle:
                playerAnimations.isIdlingTop();
                stateInfo = top.GetCurrentAnimatorStateInfo(0);
                break;
            case playerTopState.moving:
                playerAnimations.IsMovingTop();
                stateInfo = top.GetCurrentAnimatorStateInfo(0);
                break;
            case playerTopState.firing:
                playerAnimations.isFiringTop();
                stateInfo = top.GetCurrentAnimatorStateInfo(0);
               // Debug.Log(stateInfo.length);
                break;
            case playerTopState.interacting:
                break;
            default:
                break;
        }
    }
    //to prevent spam fire and prevent issues with firing animations
    public IEnumerator FireCoolDown()
    {
        _fireCoolDown = true;
        yield return new WaitForSeconds(fireRate);
        _fireCoolDown = false;
    }

    //when we fire, we launch the firing animation, while the bool is on (the anim is playing) we cannot switch to topIdle or topMoving until its done
    IEnumerator FireState()
    {
        animTopState = playerTopState.firing;
        firingState = true;

        //however long the animation is
        yield return new WaitForSeconds(top.GetCurrentAnimatorStateInfo(0).length);

    }
}
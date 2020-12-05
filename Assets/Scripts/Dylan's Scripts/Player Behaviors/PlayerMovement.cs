using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using JetBrains.Annotations;

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
    public AudioSource Powerup;

    //animation states for player - Since no animations, these will always be the same state. Only one animation on player.
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

    //set up for rotation and new rotation orientation
    [Header("Parent object of this player obj")]
    public GameObject parent;
    public GameObject follower;
    private GameObject[] character = new GameObject[8];

    [Header("Player default movement speed")]
    public float movementSpeed = 1.0f;
    Vector2 movementInput;
    //rotation
    [Header("Rotation Speed")]
    public float turnSpeed = 20f;
    public float lookSpeed = 30f;
    float _angle, _angle2;

    //player looking rotation
    Vector2 lookingInput;

    [Header("Current Player Stats - Set on Scene Start")]
    public float health;
    public int damage;
    public float speedMultiplier;

    [Header("Player Animation State")]
    public playerBottomState animBottomState;
    public playerTopState animTopState;
    public playerTopState _localTopState = playerTopState.idle;

    //when we hit door or go off edge
    [HideInInspector]
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
    [HideInInspector]
    public bool checkToCalculate = false;
    private Vector3 p01;

    //for smooth parent rotation
    //private Quaternion par01;
   // private Transform pc1, pc0;

    EasingType easingTypeC = EasingType.linear;
    [HideInInspector]
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
    [Header("Player Setup Obj")]
    public PlayerSetUp myPlayerSetup;

    /// <summary>
    /// Scene Transition and endgame
    /// 
    /// </summary>
    private GameObject teleporterTracker; //Assign before load, set to private if unneeded
    public Animator[] transition; //Transition animator
    public float transitionTime = 1;
    private int rng;
    [HideInInspector]
    public bool firingState;
    //set to time to run full animation before repeat, may be a bit shorter so it works better
    [HideInInspector]
    public float localTimer;

    [Header("Player Modifiers")]
    //player choosen mods
    public bool healthRegen = false;
    public bool doubleScoreMod = false;
    public bool serratedMod = false;
    public bool doubleDamage = false;
    public bool personalSheild = false;
    public bool trackingAmmunitionMod = false;
    public GameObject mortarGrid;
    //player takes damage and applies bleed, while active every second does 1 damage for 5 seconds. Each stack adds 1 damage
    public int bleedStacks = 0;
    public float bleedTimer = 0;
    bool _bleedCooldown = false;
    
    //displays timer per level (resets at level start and ends at level end
    [Header("UI")]
    public Text timerText;
    public UIInGame gamerUI;

    [Header("Player Animators")]
    public Animator top;
    public GameObject topObj;
    public GameObject anchorRef;

    //animation
    public PlayerAnimations playerAnimations;
    public AnimatorStateInfo stateInfo;
    public float shootingAnimLength = 1.5f;

    //cooldown on fire animations to avoid animation playing before its done
    bool _fireCoolDown;
    [Header("Fire Rate")]
    public float fireRate = 0.1f;
    float firingTimer = 0;

    public GameObject PlayerRotate;
    //bleeding variables for stacks and bleed debuff
    float bleedTimer2 = 0;
    bool isBleeding = false;
    bool damageStbTimer = false;
    float damageTimer = 0f;
    //sheild mod prefab
    public GameObject sheildObj;

    /// <summary>
    /// Dylan Loe
    /// Last Updated: 10-31-2020
    /// 
    /// - Sets stats to local player in level, establishes character custom, sets teleporters, rng, localtimer and starts the perSecond invokeRepeating
    /// </summary>
    void Start()
    {
        //Debug.Log(health);
        SetUpCharAppearance();

        if (playerData.godMode)
            Debug.Log("DEBUG MODE ON");

        //cap refresh rate on those computers that believe themselves to be above us
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        //if mod is on we double local damage
        if (doubleDamage)
            damage += damage;

        //commented cause it sucks

        //    for (int i = 0; i < character.Length; i++)
        //    {
        //        character[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
        //    }

        //if (serratedMod)
         //   playerData.HealthBuffSerationMod();

        //SetPlayerModifiers(); Player Mods set up in LevelSetup obj and script
        SetPlayerStats();

        teleporterTracker = GameObject.FindGameObjectWithTag("GoalCheck"); //assumes we check on construction of the player, with a new player every level - Wesley
        rng = Random.Range(0, transition.Length);
        localTimer = playerData.timerBetweenLevels;
        InvokeRepeating("PerSecond", 0f, 1f); //Every second, give score equal to 1*the level count. - Wesley
    }

    /// <summary>
    /// Dylan Loe
    /// Updated 12-3-2020
    /// 
    /// - Used Primarly for physics based movement and cube transversals
    /// </summary> 
    private void FixedUpdate()
    {
        Physics.IgnoreCollision(anchorRef.GetComponent<Collider>(), GetComponent<Collider>());
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //cant move if we are rotating
        if (!overTheEdge && !moving)
        {
            Movement();
            Looking();
        }
        else
        {
            //if we are interpolating, set to idle
            animBottomState = playerBottomState.idle;
            animTopState = playerTopState.idle;
            SetAnimation();
        }

        Timer();

        //moves player to next side of cube
        if (_rotationTrans != null)
        {
            Interpolation();
        }

        //so the looky thing does go over there
       // topObj.transform.position = this.transform.position;
        
    }

    /// <summary>
    /// Dylan Loe
    /// Updated 10-31-2020
    /// 
    /// Runs checks for detections, firing state timers, setting animation states, and bleed damage timers
    /// </summary>
    private void Update()
    {
        //sheild mod
        DamageStandbyTimer();

        //movement
        //detects if player is over an edge
        if (DetectEdge())
        {
            overTheEdge = true;
            //Debug.Log("Over edge");
        }
        else
            overTheEdge = false;
        
        //Interpolation stuff, for rotation onto next side
        if (overTheEdge && onDoor && !checkToCalculate && !moving)
        {
            //if we hit the door and are off the cube

            SwapRotate();
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
           // SetAnimation();
        }

       //bleed stacks can only be active when bleed mod is on
       if(isBleeding && bleedStacks > 0)
       {
            bleedTimer += Time.deltaTime;
            bleedTimer2 += Time.deltaTime;
            if (bleedStacks >= 5)
                playerData.GiveRedDead();

            if(bleedTimer2 >= 1.0)
            {
                bleedTimer2 = 0;
                Debug.Log("took bleed damage");
                
                //do damage based on stacks
                health -= 1 * bleedStacks;
                gamerUI.healthBarStatus(health);
            }
            else if(bleedTimer >= 5.0)
            {
                Debug.Log("Done bleeding");
                bleedStacks = 0;
                bleedTimer = 0;
                isBleeding = false;
                health -= 1 * bleedStacks;
                gamerUI.healthBarStatus(health);
            }
       }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated 11-22-2020
    /// 
    /// Sets up animator, and rig for char
    /// </summary>
    void SetUpCharAppearance()
    {
        if(!playerData.characterModelSwitch)
        {
            //set up char 1
            this.GetComponent<Animator>().runtimeAnimatorController = myPlayerSetup.p1Controller;
            this.GetComponent<Animator>().avatar = myPlayerSetup.p1Avatar;
        }
        else
        {
            //set up char 2
            this.GetComponent<Animator>().runtimeAnimatorController = myPlayerSetup.p2Controller;
            this.GetComponent<Animator>().avatar = myPlayerSetup.p2Avatar;
        }

    }

    /// <summary>
    /// Dylan Loe
    /// Updated 10-31-2020
    /// 
    /// Initial setup for interpolations
    /// </summary>
    void SwapRotate()
    {
        //PlayerRotate.transform.position = this.transform.position;
        //rotates the parent of both playerObj and PlayerFollower
        PlayerRotate.transform.parent = null;

        PlayerRotate.transform.eulerAngles = _startingT.eulerAngles;

        //make sure PlayerRotate is at 0, 0, 0 and ect for each door

        transform.parent = PlayerRotate.transform;


        transform.localPosition = Vector3.zero;
        //this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Collider>().isTrigger = true;

        follower.transform.parent = PlayerRotate.transform;
        //Debug.Log(follower.transform.localEulerAngles);
        //follower.transform.localEulerAngles = 
       // PlayerRotate.transform.localRotation = follower.transform.localRotation;


        //follower.transform.localEulerAngles = Vector3.zero;
        checkToCalculate = true;
    }


    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Used for interpolations for cube side transverals
    /// - moves player based on equation
    /// </summary>
    void Interpolation()
    {
        //rotate parent to same degree 

        if (checkToCalculate)
        {
            c0 = PlayerRotate.transform;//this.transform;
            c1 = _rotationTrans;
            
            checkToCalculate = false;
            moving = true;
            timeStart = Time.time;
        }

        if (moving)
        {
            u = (Time.time - timeStart) / timeDuration;

            if (u >= 0.7) //originally 1 - lowered u to speed up time, the small movements at the end can be essentially skipped maybe
            {
                PlayerRotate.transform.position = _rotationTrans.transform.position;

                //when we reach new pos
                parent.transform.rotation = _rotationTrans.transform.rotation;
                //so player does go to high
                this.transform.position = _rotationTrans.transform.position;

                this.transform.parent = parent.transform;
                follower.transform.parent = parent.transform;

                //Debug.Log("set this obj parent back to ParentObj");
                PlayerRotate.transform.parent = this.transform;
                //PlayerRotate.transform.position = Vector3.zero;

                //PlayerRotate.transform.localPosition = Vector3.zero;
                PlayerRotate.transform.localEulerAngles = Vector3.zero;
                PlayerRotate.transform.position = this.transform.position;
                
               //this.GetComponent<Rigidbody>().constraints.

                u = 1;
                moving = false;
                _rotationTrans = null;
                overTheEdge = false;
                // Debug.Log(overTheEdge);
                //this.GetComponent<Rigidbody>().isKinematic = false;
                this.GetComponent<Collider>().isTrigger = false;
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

           // par01 = Quaternion.Slerp(pc0.rotation, pc1.rotation, u2);

            //apply those new values
            PlayerRotate.transform.position = p01;
            PlayerRotate.transform.rotation = r01;
            //follower.transform.rotation = r01;
            //parent.transform.rotation = par01;
            //this.transform.localPosition = Vector3.zero;
        }
    }
   
    
    /*
     * Movement and Player Looking Rotation
     * Uses player controller and both joysticks for movement and for player sights
     */
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Assigns movementInput vector from playercontroller for joystick movement
    /// </summary>
    private void OnMove1(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Assigns lookingInput vector from playercontroller for joysticking looking (for firing)
    /// </summary>
    private void OnLook(InputValue value)
    {
        lookingInput = value.Get<Vector2>();
       // Debug.Log(lookingInput);
    }

    //joysticks
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Takes x value from horizontal movementinput vector and clamps between -1 and 1
    /// </summary>
    public float JoystickH()
    {
        float h = movementInput.x;
        return Mathf.Clamp(h, -1.0f, 1.0f);
    }
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Takes x value from vertical movementinput vector and clamps between -1 and 1
    /// </summary>
    public float JoystickV()
    {
        //float r = 0.0f;
        float v = movementInput.y;
        //r += Mathf.Round(v);
        return Mathf.Clamp(v, -1.0f, 1.0f);
    }
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Returns vector2 from both clampsed values
    /// </summary>
    public Vector2 MainJoystick()
    {
        return new Vector2(JoystickH(), JoystickV());
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Takes in vector2 movement as applies this to the player for rotational movement
    /// - turns this value into a degree and assigns it 
    /// - also has speedModifiers
    /// - usss lerp for smoother rotations
    /// </summary>
    void RotateMovement(Vector2 movement)
    {
        //convert joystick movements to angles that we can apply to player rotation
        _angle = Mathf.Atan2(movement.x, movement.y);
        _angle = Mathf.Rad2Deg * _angle;


        float newAngle = _angle + follower.transform.localEulerAngles.y;
        //local angles are used since its a child, the player parent is set to keep track of the global rotation
        transform.localRotation = Quaternion.Euler(0 , Mathf.LerpAngle(transform.localEulerAngles.y, newAngle, Time.deltaTime * turnSpeed), 0 ); 

        // transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, Mathf.LerpAngle(this.gameObject.transform.localEulerAngles.y, _angle, Time.deltaTime * turnSpeed), transform.localEulerAngles.z);

        //base movement is just 1.0
        float boost = movementSpeed * speedMultiplier;
        float newSpeed = movementSpeed + boost;

        //player is always moving forward, player is just adjsuting which way they move forward (always local forward so we can have player move consistentaly forward on each side)
        transform.position += transform.forward * newSpeed *speedAdjustment()* Time.deltaTime;
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// takes in movementintput and applies deadzone as well as animation states. Runs RotateMovement to apply to player
    /// </summary>
    void Movement()
    {
        float deadzone = 0.35f;
        Vector2 movement = MainJoystick();
        //Debug.Log(movement);
        if (movement.magnitude < deadzone)
        {
            movement = Vector2.zero;
        }


        //only move if player gives input
        if (movement != Vector2.zero)
        {
            RotateMovement(movement);
            animBottomState = playerBottomState.walking;
            //turned off temp cause we dont have animations lol
            //if(!firingState)
                animTopState = playerTopState.moving;
            //if(animTopState != playerTopState.moving)
                SetAnimation();
        }
        else
        {
            animBottomState = playerBottomState.idle;
            //if (!firingState)
            animTopState = playerTopState.idle;

            //if (animTopState != playerTopState.idle)
            SetAnimation();
        }
    }

    /// <summary>
    /// Zach and Dylan
    /// Updated: 10-20-2020
    /// 
    /// - make movement not constant
    /// </summary>
    public float speedAdjustment()
    {
        float speedMod = Mathf.Sqrt((movementInput.x * movementInput.x) + (movementInput.y * movementInput.y));

        //player legs walking speed is determined here
        //SET ANIMATION SPEED HERE (multiply anim speed by speed mod)

        return speedMod;
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Takes x value from horizontal lookinginput vector and clamps between -1 and 1
    /// </summary>
    public float JoystickLookingH()
    {
        float h = lookingInput.x;
        return Mathf.Clamp(h, -1.0f, 1.0f);
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Takes x value from horizontal lookinginput vector and clamps between -1 and 1
    /// </summary>
    public float JoystickLookingV()
    {
        float v = lookingInput.y;
        return Mathf.Clamp(v, -1.0f, 1.0f);
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Returns vector2 from both clampsed values
    /// </summary>
    public Vector2 LookJoystick()
    {
        return new Vector3(JoystickLookingH(), JoystickLookingV());
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// takes in lookingintput and applies deadzone as well as animation states. Runs LookMovement to apply to player
    /// </summary>
    void Looking()
    {
        float deadzone = 0.25f;
        Vector2 looking = LookJoystick();
        //Debug.Log(movement);
        //only move if player gives input

        //deadzones (high precision or radial?)
        //RADIAL DEADZONE
        if(looking.magnitude < deadzone)
        {
            looking = Vector2.zero;
        }

        if (looking != Vector2.zero)
        {
            LookMovement(looking);
            //animTopState = playerTopState.moving;
        }
      //  else
           // animTopState = playerTopState.idle;
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Takes in vector2 movement as applies this to the player for rotational looking movement
    /// - turns this value into a degree and assigns it 
    /// - uses lerp for smoother rotations
    /// - runs attacking when active
    /// </summary>
    void LookMovement(Vector2 looking)
    {
        //convert joystick movements to angles that we can apply to player rotation
        _angle2 = Mathf.Atan2(looking.x, looking.y);
        _angle2 = Mathf.Rad2Deg * _angle2;
        //Debug.Log(_angle);
        //local angles are used since its a child, the player parent is set to keep track of the global rotation
        //rotates top half with the gun
        float newAngle = _angle2 + follower.transform.localEulerAngles.y;
        topObj.transform.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(topObj.transform.localEulerAngles.y, newAngle, Time.deltaTime * lookSpeed), 0);


        //FIRE HERE
        OnStickAttack();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Returns bool if player crosses an edge for side transversal
    /// </summary>
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
           // Debug.Log("Found edge");
        }
        else
            noFloor = true;

        return noFloor;
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Player attack if cooldown is off
    /// - fires prefab based on looking rotation (not mvoement rotation)
    /// - runs when player is using right joystick or right sholder button
    /// </summary>
    void OnStickAttack()
    {
        if (!_fireCoolDown)
        {
            //runs everytime our char attacks
            //Wesley-Code
            GameObject bullet = Object.Instantiate(Player_Bullet, anchorRef.transform.position, topObj.transform.rotation);

            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
            if (personalSheild)
                Physics.IgnoreCollision(bullet.GetComponent<Collider>(), sheildObj.GetComponent<Collider>());
            //ZACHARY ADDED THIS
            //

            if(trackingAmmunitionMod)
                bullet.GetComponent<ProjectileScript>().smartBullets = true;

            //just to destroy stray bullets if they escape the walls
            
            // _repeatedFire = true;
            //when we fire we run fire animation once (firing state is active while animation is active)
            // StartCoroutine(FireState());

            //start animation
            firingState = true;
            //Debug.Log("fire");
            //temporary turned off cause i dont have animations (will ignore firing for now)
            //animTopState = playerTopState.firing;
            playerAnimations.isFiringTop();
            firingTimer = 0;
            StartCoroutine(FireCoolDown());
        }
    }

    //triggers
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Handles triggers related to Door tag, goal tag, bullet tag, enemy tags and powerup tags
    /// </summary>
    Transform _startingT;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Door")
        {
            onDoor = true;
            _rotationTrans = other.gameObject.GetComponent<DoorTrigger>().moveLocation;
            _startingT = other.gameObject.GetComponent<DoorTrigger>()._starting;
            //_startingT = other.gameObject.GetComponent<DoorTrigger>().OnHit();
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
            //takeDamage(bulletDamage); //Olivia changed this
            takeDamage(other.GetComponent<TurretBullet>().damage);

            Debug.Log("Current health: " + health);
        }

        ///added by Christian to take damage when colliding with an enemy
        if (other.gameObject.tag == "Enemy")
        {
            //check if I'm colliding with an actual enemy or just a bullet
            if (other.TryGetComponent<BaseEnemy>(out BaseEnemy BE)) {
                takeDamage(BE.damage);

                //could apply a random percentage of extra damage for future iterations
                if(other.gameObject.GetComponent<BaseEnemy>().doubleDamageMod) {
                    takeDamage(other.GetComponent<BaseEnemy>().damage);
                }

                //Debug.Log("Current health: " + health);
            }
            else if (other.TryGetComponent<EnemyProjectile>(out EnemyProjectile EP)) {
                takeDamage(EP.damage);
                other.gameObject.SetActive(false);
            }
            //check if I'm colliding with the enemy or the bullet


        }

        if(other.gameObject.tag == "PowerUp")
        {
            //Debug.Log("Hit powerup");
            PickedPowerUp(other.gameObject.GetComponent<PowerUpDrop>().type, other.gameObject.GetComponent<PowerUpDrop>().timer, other.gameObject.GetComponent<PowerUpDrop>().powerUpDuration);
            //run animation on powerup (if any)
            playerData.TrackPowerupGains(1, other.gameObject.GetComponent<PowerUpDrop>().type);
            Powerup.Play();
            Destroy(other.gameObject);
        }
           
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Handles when player leaves door tag for cube transveral
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Door")
        {
            onDoor = false;
        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// For changing easing types (equations) for interpolation testing
    /// </summary>
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

    /// <summary>
    /// Dylan Loe & Whesley
    /// Updated 10-25-2020
    /// 
    /// Sets local player info from playerdata obj as well as gamerUI (on start)
    /// </summary>
    void SetPlayerStats()
    {
        
        health = playerData.localHealth;

        damage = playerData.totalDamageBase + playerData.damageUpgrade;

        gamerUI.healthBarStatus(health);

        speedMultiplier += (playerData.speedUpgrade)/20;

        //if pet is on add stats here
        switch (playerData.petChoice)
        {
            case 0:
                //no pet
                break;
            case 1:
                //hornet
                damage += 5;
                Debug.Log("Hornet Buff");
                break;
            case 2:
                //bunny
                speedMultiplier += 0.5f;
                Debug.Log("Bunny Buff");
                break;
            default:
                break;
        }

        //Wesley
        playerData.SetCharacterChoiceGame();
        playerData.SetColor(); //Sets in scene start
        playerData.SetPet();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated 10-25-2020
    /// 
    /// When player takes damage, remove health
    /// - bleeding mod (coroutine starts here)
    /// - if health 0 then gameover
    /// </summary>
    public void takeDamage(int damageTaken)
    {
        //if player has debug mode on, player cant die
        if (!playerData.godMode)
        {
            
            //damage player
            if (!serratedMod)
                health -= damageTaken;
            else if (bleedStacks <= 5)
            {
                //start bleed out
                if (!_bleedCooldown)
                {
                    Debug.Log("ow");
                    isBleeding = true;
                    bleedStacks++;
                    StartCoroutine(BleedDamage());
                }
            }
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
                playerData.AddDeath();

                UnityEngine.SceneManagement.SceneManager.LoadScene(7);
                //DontDestroyOnLoad(GameObject.Find("ScriptManager"));
            }

            SheildRegenStop();
        }
    }

    /// <summary>
    /// Whesley and Dylan
    /// Updated: 10-20-2020
    /// 
    /// when level is beat, scene transitions and run BeatLevel function from playerdata
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadTargetLevel()
    {
        transition[rng].SetTrigger("Start"); //start animation

        yield return new WaitForSeconds(transitionTime); //Time given for transition animation to play

        //SceneManager.LoadScene(nextScene); //Loads target scene
        playerData.BeatLevel();
    }

    //Score - Wesley
    /// <summary>
    /// Wesley and Dylan
    /// Updated: 10-20-2020
    /// 
    /// Each second, adds score based on modifiers
    /// - heath regen timer also occurs here
    /// </summary>
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

    /// <summary>
    /// Dylan Loe
    /// Updated 10-20-2020
    /// 
    /// Runs and collects total time, applies this to playerData
    /// - updats up as well
    /// </summary>
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
        DisplayTime();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Updates timer in specific format
    /// </summary>
    void DisplayTime()
    {
        //update text here with info from playerdata
       // const string Format = "{22:11:00}";
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", playerData.timerHour, playerData.timerMin, playerData.timerSec);

    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Counts with timer and updates to playerData
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// when player gets a powerup from ontrigger, runs this which passes info to start a corouinte for duration
    /// </summary>
    void PickedPowerUp(powerUpType type, bool timer, int duration)
    {
        
        if(timer)
        {
            StartCoroutine(PowerUpDuration(type, duration));
        }
    }
    
    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Depending on powerup, certain effects happen such as damage/speed buff or increase to health
    /// </summary>
    IEnumerator PowerUpDuration(powerUpType type, float duration)
    {
        //turn on
        switch (type)
        {
            case powerUpType.damage:
                gamerUI.vDamage.SetActive(true);
                damage += 5;
                break;
            case powerUpType.speed:
                gamerUI.vSpeed.SetActive(true);
                speedMultiplier += 0.5f;
               // Debug.Log(speedMultiplier);
                break;
            case powerUpType.health:
                gamerUI.vHealth.SetActive(true);
                duration = .5f;

                if (serratedMod)
                    playerData.totalHealthBase++;
              //  else
               // {
                    if (health < playerData.totalHealthBase)
                    {
                        health += 20;
                        playerData.localHealth = health;

                    }
                    if (health > playerData.totalHealthBase)
                        health = playerData.totalHealthBase;
              //  }
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
                gamerUI.vDamage.SetActive(false);
                damage -= 5;
                break;
            case powerUpType.speed:
                gamerUI.vSpeed.SetActive(false);
                speedMultiplier -= 0.5f;
                break;
            case powerUpType.health:
                gamerUI.vHealth.SetActive(false);
                break;
            default:
                break;
        }
        //Debug.Log("off");
        //Debug.Log(speedMultiplier);
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Runs animations based on what state animatoins are in
    /// - CURRENTLY NOT IN USE BECAUSE WE DONT HAVE ANIMATIONS TO RUN
    /// </summary>
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

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Starts a cooldown to prevent to rapid of firing, prevents possible issues with firing animations looping on top as well
    /// </summary>
    /// <returns></returns>
    public IEnumerator FireCoolDown()
    {
        _fireCoolDown = true;
        yield return new WaitForSeconds(fireRate);
        _fireCoolDown = false;
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// When firing, sets firing anim state and waits for anim duration int before returning to other states
    /// - when we fire, we launch the firing animation, while the bool is on (the anim is playing) we cannot switch to topIdle or topMoving until its done
    /// </summary>
    /// <returns></returns>
    IEnumerator FireState()
    {
        animTopState = playerTopState.firing;
        firingState = true;

        //however long the animation is
        yield return new WaitForSeconds(top.GetCurrentAnimatorStateInfo(0).length);

    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// When player takes damage with mod on, we run 1 bleeding effect stack that lasts for 5, IT CAN STACK FOR MORE DAMAGE
    /// </summary>
    /// <returns></returns>
    IEnumerator BleedDamage()
    {
        StartCoroutine(BleedApplyCooldown());
        bleedTimer = 0;
        //bleedStacks++;
        yield return new WaitForSeconds(5.0f);
        //bleedStacks = 0;
        //bleedStacks--;
    }



    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Activates a fast cooldown to prevent multiple damage stacks being applied to fast (so its fair i guess)
    /// </summary>
    /// <returns></returns>
    IEnumerator BleedApplyCooldown()
    {
        _bleedCooldown = true;
        yield return new WaitForSeconds(0.5f);
        _bleedCooldown = false;
    }


    
    /// <summary>
    /// Dylan Loe
    /// Updated: 11-5-2020
    /// 
    /// - if player takes damage, begin timer, once timer is reached sheild can regen health
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
    /// Updated: 11-5-2020
    /// 
    /// this runs when player takes damage
    /// </summary>
    public void SheildRegenStop()
    {
        damageStbTimer = true;
        sheildObj.GetComponent<ForceFieldsEnemy>().ableToRecharge = false;
    }
}
//Wesley Morrison
//9/7/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed; //speed modifiable by level designer
    private int damage; //How much damage the projectile does, modifiable by power ups, get from player logic
    private GameObject playerRef; //reference to player

    public bool smartBullets = false;
    public float smartCheckRadius = 1.0f;

    public GameObject target;
    public GameObject tracker;
    Rigidbody rb;

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Sets inital vars and adds initial force
    /// </summary>
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        damage = playerRef.GetComponent<PlayerMovement>().damage;
        Physics.IgnoreCollision(this.GetComponent<Collider>(), playerRef.GetComponent<Collider>());
        //Physics.IgnoreCollision(this.GetComponent<Collider>(), playerRef.GetComponent<PlayerMovement>().sheildObj.GetComponent<ForceFieldsEnemy>().GetComponent<Collider>());
        //StartCoroutine(destroyProjectile());
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 1000);
        StartCoroutine(destroyProjectile());
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Runs detection and direciton movement based on target
    /// </summary>
    void FixedUpdate()
    {
        //Move_Forward();
        Debug.DrawRay(transform.position, transform.forward, Color.green);

        //move angle towards target
        //if there is a target, use lerp or something to point bullet towards target
        if (smartBullets) {
           
            if(target != null)
            {
                Debug.Log(target.name);
                //Quaternion lookingRot = Quaternion.LookRotation(transform.position, Vector3.forward);
                tracker.transform.LookAt(target.transform, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, tracker.transform.rotation, Time.time * 100);

               // rb.
                rb.isKinematic = true;
                rb.isKinematic = false;
                rb.AddForce(transform.forward * 1000);
            }
            else
                CheckForTarget();
        }
    }


    void Move_Forward() //Moves projectile along axis (may later be modified to towards Target)
    {
        transform.Translate(this.transform.forward * speed * Time.deltaTime);
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Trigger collisions
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") //Assuming enemies will have this tag.
        {
            //other.GetComponent<BaseEnemy>().health -= damage; //This is normal damage
            if (other.gameObject != null)
            {
                other.GetComponent<BaseEnemy>().takeDamage(playerRef); //Christian's code
            }
            if (Random.Range(0f, 10f) < 1) //This is a crit
            {
                //Debug.Log("I hit a crit!");
                //other.GetComponent<BaseEnemy>().health -= damage;
                other.GetComponent<BaseEnemy>().takeDamage(playerRef); //Christian's code
            }
            other.GetComponent<BaseEnemy>().SheildRegenStop();
            //gameObject.SetActive(false);
            // StartCoroutine(OnDestruction());
            Destroy(this.gameObject);
            ///Debug.Log("Enemy is taking damage.");
        }
        //More of Christian's code below
        else if (other.tag == "Shield")
        {
            Destroy(this.gameObject);
            Debug.Log("I hit the shield.");
        }
        //For sheild mods on enemies
        else if (other.tag == "ShieldMod") 
        {
            Debug.Log("hit sheild");
            if (!other.gameObject.GetComponent<ForceFieldsEnemy>().inactive)
            {
                
                other.gameObject.GetComponent<ForceFieldsEnemy>().currentHealth -= damage;
                // gameObject.SetActive(false);
                //StartCoroutine(OnDestruction());
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" )
        {
            //gameObject.SetActive(false);
            //Destroy(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    //ADDED BY ZACHARY just to destroy projectiles
    public IEnumerator destroyProjectile()
    {

        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    //if there is a target in sphere detection, make it target
    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Looks for target in detection zone
    /// </summary>
    void CheckForTarget()
    {
        if(target == null)
        {
            Transform[] _nearby = collidersToTransforms(Physics.OverlapCapsule(transform.position, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 3), smartCheckRadius));
            foreach (Transform potentialTarget in _nearby)
            {
                if (potentialTarget.gameObject.tag == "Enemy" || potentialTarget.gameObject.tag == "WallTurret")
                {
                    //player in range, damage player
                    if(target == null)
                    {
                        target = potentialTarget.gameObject;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// converts any collisions to transforms
    /// </summary>
    private Transform[] collidersToTransforms(Collider[] colliders)
    {
        Transform[] transforms = new Transform[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            transforms[i] = colliders[i].transform;
        }
        return transforms;
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Show gizmos
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, smartCheckRadius);
        Gizmos.DrawWireSphere(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 3), smartCheckRadius);
    }
}

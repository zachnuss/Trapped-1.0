/*
 * Author: Christian Mullins
 * Summary: A simple script that enables enemies to shoot.
 */ 
using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
    /**
     * HOW TO USE:
     *      -Attach this script to any enemy that may use this.
     *      -Use functions in the enemys' script for when enemy may shoot.
     *      -Adjust public variables in this script.
     */ 

    //public variables
    public GameObject projectilePrefab;
    [Header("How often will the enemy shoot?")]
    public float shootCoolDown = 2f;
    //getters and setters
    public bool canShootPlayer { 
        get { return _canShootPlayer; }
        set { _canShootPlayer = value; } 
    }
    public float shootTimer {
        get { return _shootTimer; }
        set { _shootTimer = value; }
    }

    //private variables
    private bool _canShootPlayer = false;
    private float _shootTimer = 0f;
    private float _speed;
    private int _damage;
    private GameObject _fwdDirGO; //getter of child

    //public getters for the bullet
    public float speed { get { return _speed; } }
    public int damage { get { return _damage; } }
    public GameObject fwdDirGO { get { return _fwdDirGO; } }

    //For some reason cG.fwdDirGO doesn't exist until a few frames after start
    //IEnumerator Start solves this issue
    IEnumerator Start() {
        //initialize values
        CommonGuard cG = GetComponent<CommonGuard>();
        yield return new WaitForSeconds(0.1f);
        _fwdDirGO = cG.fwdDirGO;
        //assignStats(cG.speed, cG.damage);//, cG.fwdDirGO.transform.position);
        _speed = cG.speed * 2f;
        _damage = cG.damage;
    }

    void Update() {
        ///check timer for shooting
        if (Time.time > _shootTimer) {
            _canShootPlayer = true;
        }
    }

    /*      PUBLIC FUNCTIONS    */
    //shoot in front of the enemy
    public void shootPlayer() {
        //gather local vals for instantiation
        GameObject newBullet;
        Quaternion zerodQ = new Quaternion(0f, 0f, 0f, 1f);
        Vector3 fireDir = (_fwdDirGO.transform.position - transform.position);
        fireDir = fireDir.normalized;
        //instantiate
        newBullet = Instantiate(projectilePrefab, _fwdDirGO.transform.position,
                                                    zerodQ, null) as GameObject;
        //pass bullet stats off 
        newBullet.GetComponent<EnemyProjectile>().firedFrom = this;
        //newBullet.GetComponent<EnemyProjectile>().assignStats(_speed * 10f,
        //                                                      _damage,
        //                                                      fireDir);
        //set a new timer
        _shootTimer = Time.time + shootCoolDown;
    }
}

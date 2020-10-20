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
    private Vector3 _fwdDir; //getter of child

    IEnumerator Start() {
        //initialize values
        CommonGuard cG = GetComponent<CommonGuard>();
        yield return new WaitForSeconds(0.5f);
        _fwdDir = cG.fwdDirGO.transform.position;
        assignStats(cG.speed, cG.damage, cG.fwdDirGO.transform.position);
        Debug.Log("ferp");
    }

    void Update()
    {
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
        Vector3 fireDir = (_fwdDir - transform.position);
        fireDir = fireDir.normalized;
        //instantiate
        newBullet = Instantiate(projectilePrefab, _fwdDir,
                                zerodQ, null) as GameObject;
        //pass bullet speed value as 
        newBullet.GetComponent<EnemyProjectile>().assignStats(_speed,
                                                              _damage,
                                                              fireDir);
        //set a new timer
        _shootTimer = Time.time + shootCoolDown;
    }


     //call in start
     public void assignStats(float speed, int damage, Vector3 fireDir) {
        _speed = speed;
        _damage = damage;
        _fwdDir = fireDir;
     }
}

/*
 * Author: Christian Mullins
 * Summary: This script allows an enemy's bullet to be fired
 *      and given various stat values.
 */
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    //private variables that will be passed down by the firing enemy
    private float _speed;
    private int _damage;
    private Vector3 _fireDir;
    public EnemyShooting firedFrom;

    //public getters
    public int damage { get { return _damage; } }

    /**       Assign initial variables before function call      **/
    void Start() {
        //set variables
        _speed = firedFrom.speed;
        _damage = firedFrom.damage;
        _fireDir = (firedFrom.fwdDirGO.transform.position 
                    - firedFrom.transform.position).normalized;
    }

    void Update() {
        //movement of the bullet
        transform.position += _speed * _fireDir * Time.deltaTime;

        //check if I'm going to collide with the wall
        if (_didHitWall()) {
            Destroy(gameObject);
        }
    }

    //functions
    /**      Call from the shooter's script and assign       **/
    public void assignStats(float newSpeed, int newDamage, Vector3 newFireDir) {
        _speed = newSpeed;
        _damage = newDamage;
        _fireDir = newFireDir.normalized;
    }

    private bool _didHitWall() {
        RaycastHit hit;
        //raycast short distance
        if (Physics.Raycast(transform.position, _fireDir, out hit, 0.75f)) {
            //check for wall
            if (hit.transform.tag == "Wall") {
                return true;
            }
            //when enemy bullets hit player sheild
            if(hit.transform.tag == "PlayerShieldMod")
            {
                Debug.Log("hit sheild");
                if (!hit.transform.gameObject.GetComponent<ForceFieldsEnemy>().inactive)
                {

                    hit.transform.gameObject.GetComponent<ForceFieldsEnemy>().currentHealth -= damage;
                    return true;
                }
                
            }
        }
        return false;
    }
}

/*
 * Author: Christian Mullins
 * Summary: This script allows an enemy's bullet to be fired
 *      and given various stat values.
 */
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    //private variables that will be passed down by the firing enemy
    private float speed;
    private int damage;
    private Vector3 fireDir;

    /**       Assign initial variables before function call      **/
    void Awake() {
        //set variables to prevent potential errors
        speed = 0;
        damage = 0;
        fireDir = Vector3.zero;
    }

    void Update() {
        //movement of the bullet
        transform.position += speed * fireDir.normalized * Time.deltaTime;

        //check for collisiongs using raycast
        if (_isColliding()) {
            Destroy(gameObject);
        }
    }

    //functions
    /**      Call from the shooter's script and assign       **/
    public void assignStats(float newSpeed, int newDamage, Vector3 newDir) {
        speed = newSpeed;
        damage = newDamage;
        fireDir = newDir;
    }

    //not all Objects 
    private bool _isColliding() {
        RaycastHit hit;
        //check for collision using Raycasting
        if (Physics.Raycast(transform.position, fireDir, 0.75f)) {
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarShell : MonoBehaviour
{
    public int damage;
    public float speed;
    public float checkRadius = 3;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(-this.transform.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if it hits anything
        Detonation();
    }

    
    void Detonation()
    {
        //run particle effect

        //damage player
        Transform[] _nearby = collidersToTransforms(Physics.OverlapSphere(transform.position, checkRadius));
        foreach (Transform potentialTarget in _nearby)
        {
            if (potentialTarget.gameObject.tag == "Player")
            {
                //player in range, damage player
                Debug.Log("hit player");
                potentialTarget.gameObject.GetComponent<PlayerMovement>().takeDamage(damage);
            }
        }

        Destroy(this.gameObject);
    }




    private Transform[] collidersToTransforms(Collider[] colliders)
    {
        Transform[] transforms = new Transform[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            transforms[i] = colliders[i].transform;
        }
        return transforms;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        //Gizmos.color = Color.yellow;
       // Gizmos.DrawWireSphere(transform.position, bulletDetectionRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MortarShell : MonoBehaviour
{
    public int damage;
    public float speed;
    public float checkRadius = 3;
    public Vector3 rayHitTrans;

    private LineRenderer mortarLine;

    public float lineSpeed = 1.0f;
    private float distance;
    float increment;

    // Start is called before the first frame update
    void Start()
    {
        mortarLine = GetComponent<LineRenderer>();
        mortarLine.positionCount = 2;
        mortarLine.startWidth = 0.45f;
        mortarLine.SetPosition(0, this.transform.position);
        mortarLine.SetPosition(1, this.transform.position);
        CastLine();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.Translate(-this.transform.up * speed * Time.deltaTime);
        transform.position += speed * Time.deltaTime * -this.transform.up;
        //CastLine();
    }

    private void Update()
    {
        if(increment < distance)
        {
            increment += .1f / lineSpeed;

            float x = Mathf.Lerp(0, distance, increment);

            Vector3 pointA = this.transform.position;
            Vector3 pointB = rayHitTrans;

            Vector3 alongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            mortarLine.SetPosition(0, this.transform.position);
            mortarLine.SetPosition(1, alongLine);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if it hits anything
        if(other)
            Detonation();
    }

    
    void Detonation()
    {

       // Debug.Log("Detonation");
        //run particle effect

        //damage player
        Transform[] _nearby = collidersToTransforms(Physics.OverlapSphere(transform.position, checkRadius));
        foreach (Transform potentialTarget in _nearby)
        {
            if (potentialTarget.gameObject.tag == "Player")
            {
                //player in range, damage player
               // Debug.Log("hit player");
                potentialTarget.gameObject.GetComponent<PlayerMovement>().takeDamage(damage);
            }
        }

        Destroy(this.gameObject);
    }


    //mortar will cast a raycast down until it hits the ground. Line renderer will go from this.transform.position to the hit.transform.position
    void CastLine()
    {
        //mortarLine.SetPosition(0, this.transform.position);
        RaycastHit hit;
       // Debug.DrawRay(this.transform.position, -transform.up, Color.green, 10f);
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            //Debug.Log(hit.transform.name);
            mortarLine.enabled = true;
            rayHitTrans = hit.point;
            distance = Vector3.Distance(this.transform.position, rayHitTrans);
            //mortarLine.SetPosition(1, rayHitTrans);
        }
      //  else
       // {
       //    // Debug.Log("no hit");
       //     rayHitTrans = Vector3.zero;
      //      mortarLine.enabled = false;
       // }
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

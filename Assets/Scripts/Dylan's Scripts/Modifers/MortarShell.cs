﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MortarShell : MonoBehaviour
{
    public int damage;
    public float speed;
    public float checkRadius = 3;
    Vector3 rayHitTrans;

    LineRenderer mortarLine;

    public float lineSpeed = 1.0f;
    float distance;
    float increment;

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Sets line renderer inital values
    /// </summary>
    void Start()
    {
        mortarLine = GetComponent<LineRenderer>();
        mortarLine.positionCount = 2;
        mortarLine.startWidth = 0.45f;
        mortarLine.SetPosition(0, this.transform.position);
        mortarLine.SetPosition(1, this.transform.position);
        CastLine();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Moves downward 
    /// </summary>
    void FixedUpdate()
    {
        //transform.Translate(-this.transform.up * speed * Time.deltaTime);
        transform.position += speed * Time.deltaTime * -this.transform.up;
        //CastLine();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Animate line as it moves downward
    /// </summary>
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

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// If it hits anything
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        //if it hits anything
        if(other)
            Detonation();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Looks for player in detection then damages them
    /// </summary>
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


    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Starts line detection based on if there is ground under them. Mortar will cast a raycast down until it hits the ground. Line renderer will go from this.transform.position to the hit.transform.position
    /// </summary>
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
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// takes colliders into transforms
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
    /// Uodated: 11-12-2020
    /// 
    /// Show gizmos in scenen
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        //Gizmos.color = Color.yellow;
       // Gizmos.DrawWireSphere(transform.position, bulletDetectionRadius);
    }
}
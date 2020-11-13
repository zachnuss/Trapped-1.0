﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetection : MonoBehaviour
{
    public DoorTrigger parent;

    /// <summary>
    /// FUTURE IMPLEMENTATION
    /// 
    /// HAVE THESE DETECTIONS SET WHICH AXIS IS LOCKED
    /// </summary>

    //trans1 = true, etc
    public bool trans = true;

    public Transform starting;

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// If player hits trigger then we set the direction of which way the player is going for cube trasnversal
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
                        /** START CHRISTIAN'S CODE **/
            DoorTrigger parentDT = transform.parent.gameObject.GetComponent<DoorTrigger>();
            if (parentDT.isTransitioning) {
                //Debug.Log("TRANSITIONING");
                CubemapFace faceA = parentDT.faces[0];
                CubemapFace faceB = parentDT.faces[1];
                CubemapFace oldFace = EnemyListener.Instance.curFace;
                //Debug.Log("-----------------------------------");
                //Debug.Log("faceA: " + faceA);
                //Debug.Log("faceB: " + faceB);

                if (faceA == oldFace) {
                    EnemyListener.Instance.setEnemiesOnFace(faceB);
                }
                else {// if (faceB == oldFace) {
                    EnemyListener.Instance.setEnemiesOnFace(faceA);
                }
                parentDT.isTransitioning = false;
            }
            /** END CHRISTIAN'S CODE **/
        }
        if(other.gameObject.tag == "Player" && !other.GetComponent<PlayerMovement>().checkToCalculate && !other.GetComponent<PlayerMovement>().moving)
        {

            //Debug.Log("hit");
            if (trans)
            {
                //Debug.Log("setting true");
                parent.direction = true;

            }
            else
            {
               // Debug.Log("Setting false");
                parent.direction = false;
            }
        }
    }

   // Transform temp;
    public Transform OnHit()
    {
        Transform temp = Instantiate(this.transform);
        temp.transform.parent = this.transform;
      //  if (direction)
            temp.localEulerAngles = new Vector3(temp.localEulerAngles.x, temp.localEulerAngles.y - 180, temp.localEulerAngles.z);
       // else
       //     temp.localEulerAngles = new Vector3(_starting.localEulerAngles.x - 180, _starting.localEulerAngles.y, _starting.localEulerAngles.z);
        Debug.Log(temp.localEulerAngles);
        return temp;
    }
}

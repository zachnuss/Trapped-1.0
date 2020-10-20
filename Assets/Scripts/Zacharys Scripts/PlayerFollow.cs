using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    private GameObject player;
   // public bool playerCenter = false;
    // Start is called before the first frame update
    void Start()
    {
       // if (!playerCenter)
            player = GameObject.FindGameObjectWithTag("Player");
      //  else
           // player = GameObject.FindGameObjectWithTag("PlayerCenterReferenceTag");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    private GameObject player;
    public float offset;
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
        transform.position = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + offset, player.transform.localPosition.z);

    }

    
}

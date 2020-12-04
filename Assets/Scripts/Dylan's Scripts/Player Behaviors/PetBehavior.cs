using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBehavior : MonoBehaviour
{
    bool yDir = false;
    float time = 0.5f;
    float speedVar = 0.25f;

    public bool hornet;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ymove());
    }

    void FixedUpdate()
    {
        if (yDir)
            transform.localPosition += new Vector3(0, speedVar * Time.deltaTime, 0);
        else
            transform.localPosition += new Vector3(0, -speedVar * Time.deltaTime, 0);

        //transform.Rotate(0, 0.25f, 0, Space.Self);
    }

    //move in y dir
    IEnumerator ymove()
    {
        
        yield return new WaitForSeconds(time);

        if (yDir)
        {
            yDir = false;
            if (hornet)
            {
                time = Random.Range(0.3f, 0.7f);
                speedVar = Random.Range(0.15f, 0.25f);
            }
            else
            {
                time = Random.Range(0.3f, 0.5f);
                speedVar = Random.Range(0.2f, 0.3f);
            }

        }
        else
            yDir = true;

        StartCoroutine(ymove());

    }
}

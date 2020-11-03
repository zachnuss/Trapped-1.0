using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform moveLocation;
   // public Transform moveMid;

    public bool direction = true;

    public GameObject pos1;
    public GameObject pos2;

    //sets starting point on player roation obj to movelocation - 180 degrees on local y
   // public Transform starting;
    public Transform _starting;
    Transform temp;


    public Transform starting1;
    public Transform starting2;
    //public Transform starting2;

    //public Transform mid1;
    //public Transform mid2;

    //future idea: add one way doors that players have to manually turn on and off or switch which direcitons they can go

    private void Update()
    {
        if (direction)
        {
            moveLocation = pos1.transform;
            _starting = starting1;
           // temp = _starting;
            //temp.localEulerAngles = new Vector3(temp.localEulerAngles.x, temp.localEulerAngles.y - 180, temp.localEulerAngles.z);
            //_starting = temp;
            //Debug.Log(starting.localEulerAngles);

            //moveMid = mid1;
        }
        else
        {
            moveLocation = pos2.transform;
            _starting = starting2;
           // temp = _starting;
            //temp.localEulerAngles = new Vector3(temp.localEulerAngles.x, temp.localEulerAngles.y - 180, temp.localEulerAngles.z);
           // _starting = temp;
            //moveMid = mid2;
        }
       // Debug.Log(this.name + ": " +starting.localEulerAngles);
    }

    public Transform OnHit()
    {
        temp = Instantiate(_starting);
        temp.transform.parent = this.transform;
        if(direction)
            temp.localEulerAngles = new Vector3(_starting.localEulerAngles.x, _starting.localEulerAngles.y - 180, _starting.localEulerAngles.z);
        else
            temp.localEulerAngles = new Vector3(_starting.localEulerAngles.x - 180, _starting.localEulerAngles.y, _starting.localEulerAngles.z);
        Debug.Log(temp.localEulerAngles);
        return temp;
    }
}

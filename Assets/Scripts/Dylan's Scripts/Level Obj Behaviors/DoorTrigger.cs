using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dylan Loe
/// 
/// Each side of cube has 4 door triggers. When player hits one, it reads this script to determine which direction we are going.
/// </summary>
public class DoorTrigger : MonoBehaviour
{
    public Transform moveLocation;

    public bool direction = true;

    public GameObject pos1;
    public GameObject pos2;

    //sets starting point on player roation obj to movelocation - 180 degrees on local y
    public Transform _starting;
    Transform temp;


    public Transform starting1;
    public Transform starting2;


    //future idea: add one way doors that players have to manually turn on and off or switch which direcitons they can go

    /** BEGIN CHRISTIAN'S CODE **/
    //This code will determine the two faces that the door is the barrier between
    //and tells the EnemyListener which face we're transitioning to so that the
    //appropriate enemies are seen on screen.
    //public getter
    public CubemapFace[] faces { get { return _faces; } }
    [HideInInspector]
    public bool isTransitioning;
    //private
    private CubemapFace[] _faces;

    void Start()
    {
        isTransitioning = false;
        _faces = new CubemapFace[] { CubemapFace.Unknown, CubemapFace.Unknown };
        //calculate my faceA and faceB
        //Assume (0f, 0f, 0f) is the center of the cube
        int faceInt;
        for (int i = 0; i < 3; ++i) {
            if (Mathf.Abs(transform.localPosition[i]) > 20f) {
                switch (i) {
                    //x axis
                    case 0:
                        faceInt = (_faces[0] == CubemapFace.Unknown) ? 0 : 1;
                        if (_faces[faceInt] == CubemapFace.Unknown) {
                            _faces[faceInt] = (transform.localPosition[0] > 0f) ?
                                              CubemapFace.PositiveX 
                                               : CubemapFace.NegativeX;
                        }
                        break;
                    //y axis
                    case 1:
                        faceInt = (_faces[0] == CubemapFace.Unknown) ? 0 : 1;
                        if (_faces[faceInt] == CubemapFace.Unknown) {
                            _faces[faceInt] = (transform.localPosition[1] > 0f) ?
                                              CubemapFace.PositiveY
                                               : CubemapFace.NegativeY;
                        }
                        break;
                    //z axis
                    case 2:
                        faceInt = (_faces[0] == CubemapFace.Unknown) ? 0 : 1;
                        if (_faces[faceInt] == CubemapFace.Unknown) {
                            _faces[faceInt] = (transform.localPosition[2] > 0f) ?
                                               CubemapFace.PositiveZ 
                                               : CubemapFace.NegativeZ;
                        }
                        break;
                }
            }
        }
        //Debug.Log("Face[0]: " + _faces[0] + ", Face[1]: " + _faces[1]);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            isTransitioning = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            isTransitioning = false;
        }
    }

    /** END CHRISTIAN'S CODE **/

    private void Update()
    {
        if (direction)
        {
            moveLocation = pos1.transform;
            _starting = starting1;
        }
        else
        {
            moveLocation = pos2.transform;
            _starting = starting2;
        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 12-13-2020
    /// 
    /// Start rotating based on either -180 degrees or +180 degrees on local Euler Angle on either y 
    /// axis or x axis depending on what side the player was on previously.
    /// </summary>
    /// <returns></returns>
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

/*
 * Author: Christian Mullins
 * Summary: This script will look for updated values relating to enemies
 *      and will update things on the enemies based on changing values.
 */ 
using UnityEngine;
using System.Collections.Generic;

public class EnemyListener : MonoBehaviour {
    //call upon a give face side like: _enemyList[(int)CubemapFace.PositiveX]
    private LinkedList<GameObject>[] _enemyList;
    private CubemapFace _curFace;
    private GameObject _playerGO;

    void Start() {
        //fill matrix
        _enemyList = new LinkedList<GameObject>[6];
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Enemy");
        int enemyListLength = tempList.Length;
        for (int i = 0; i < enemyListLength; ++i) {
            int enumIndex = (int) tempList[i].GetComponent<BaseEnemy>().myFaceLocation;
            _enemyList[enumIndex].AddFirst(tempList[i]);
            //set active false if enemy is not on PositiveY
            if (enumIndex != (int) CubemapFace.PositiveY) {
                tempList[i].SetActive(false);
            }
        }
        //assuming the player always starts on the PositiveY
        _curFace = CubemapFace.PositiveY;

        _playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    /**
     * To Do:
     *      -Figure out a way to set a value to each door collider so that
     *      -When the player collides with it, setEnemiesOnFace()
     * 
     */ 

    void Update() {
        //get player's current CubemapFace data
    }

    //replace curFace with the argument parameter and sets all enemies on face
    //to active, and set enemies on oldFace to inactive
    public void setEnemiesOnFace(CubemapFace face) {
        //swap enums
        CubemapFace oldFace = _curFace;
        _curFace = face;
        //set enemys in new face to active
        if (_enemyList[(int)_curFace].Count > 0) {
            var nodeIter = _enemyList[(int)_curFace].First;
            do {
                nodeIter.Value.SetActive(true);
                nodeIter = nodeIter.Next;
            } while (nodeIter != null);
        }
        //set enemies in old face to inactive
        if (_enemyList[(int)oldFace].Count > 0) {
            var nodeIter = _enemyList[(int)oldFace].First;
            do {
                nodeIter.Value.SetActive(false);
                nodeIter = nodeIter.Next;
            } while (nodeIter != null);
        }
    }

    //public function that will delete a give enemy from the array
    public void deleteFromList(GameObject delete) {
        //assuming the killed enemy is on the same face as the player
        _enemyList[(int)_curFace].Remove(delete);
    }

    //calculate the current face of the player so we know what enemies
    //to display
    private CubemapFace _getCurrentFace() {
        Vector3 playerUp = _playerGO.transform.up;
        CubemapFace newFace = CubemapFace.Unknown;
        if (playerUp[0] != 0f) {
            newFace = (playerUp[0] > 0f) ? CubemapFace.PositiveX
                                         : CubemapFace.NegativeX;
        }
        else if (playerUp[1] != 0f) {
            newFace = (playerUp[1] > 0f) ? CubemapFace.PositiveY
                                         : CubemapFace.NegativeY;
        }
        else if (playerUp[2] != 0f) {
            newFace = (playerUp[2] > 0f) ? CubemapFace.PositiveZ
                                         : CubemapFace.NegativeZ;
        }
        return newFace;
    }
}

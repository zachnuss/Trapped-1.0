/*
 * Author: Christian Mullins
 * Summary: This script will look for updated values relating to enemies
 *      and will update things on the enemies based on changing values.
 */ 
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyListener : MonoBehaviour {

    //singleton
    public static EnemyListener Instance;
    public CubemapFace curFace { get { return _curFace; } }
    //call upon a give face side like: _enemyList[(int)CubemapFace.PositiveX]
    private List<BaseEnemy>[] _enemyList;
    private CubemapFace _curFace;
    private GameObject _playerGO;
    
    //two part Start()
    IEnumerator Start() {
        Instance = this;
        //assuming the player always starts on the PositiveY
        _curFace = CubemapFace.Unknown;
        _playerGO = GameObject.FindGameObjectWithTag("Player");

        //fill matrix empty
        _enemyList = new List<BaseEnemy>[6];
        for (int i = 0; i < 6; ++i) {
            _enemyList[i] = new List<BaseEnemy>();
        }
        yield return new WaitForEndOfFrame();
        //wait a frame before callling <BaseEnemy>().myFace as enemies
        //calculate this variable in Start() as well.
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("Enemy");
        int enemyListLength = tempList.Length;
        foreach (var enemy in tempList) {
            int enumIndex = (int) enemy.GetComponent<BaseEnemy>().myFaceLocation;
            _enemyList[enumIndex].Add(enemy.GetComponent<BaseEnemy>());
           /* Debug.Log("Adding: " + enemy.name + "(" + enemy.transform.parent.name + ")" +
                      ", Face: " + enemy.GetComponent<BaseEnemy>().myFaceLocation);
            *///enemy.SetActive(false);
            //if (enumIndex != (int)_curFace) {
                enemy.GetComponent<BaseEnemy>().activateAI(false);
            //}
        }
        /*int activeEnemyCount = _enemyList[(int)_curFace].Count;
        for (int i = 0; i < activeEnemyCount; ++i) {
            _enemyList[(int)_curFace][i].activateAI(true);
        }*/
        setEnemiesOnFace(CubemapFace.PositiveY);
        _curFace = CubemapFace.PositiveY;

    }

    //replace curFace with the argument parameter and sets all enemies on face
    //to active, and set enemies on oldFace to inactive
    public void setEnemiesOnFace(CubemapFace face) {
        //swap enums
        CubemapFace oldFace = _curFace;
        _curFace = face;
        //set enemys in new face to active
        int curFaceCount = _enemyList[(int)_curFace].Count;
        if (curFaceCount > 0) {
            //Debug.Log("ACTIVATING: " + curFaceCount);
            for (int i = 0; i < curFaceCount; ++i) {
                //_enemyList[(int)_curFace][i].SetActive(true);
                _enemyList[(int)_curFace][i].activateAI(true);
                //Debug.Log("Activating: " + _enemyList[(int)_curFace][i].gameObject.name);
            }
        }
        if ((int)oldFace == -1) return;
        //set enemies in old face to inactive
        int oldFaceCount = _enemyList[(int)oldFace].Count;
        if (oldFaceCount > 0) {
            //Debug.Log("DEACTIVATING: " + oldFaceCount);
            for (int i = 0; i < oldFaceCount; ++i) {
                //_enemyList[(int)oldFace][i].SetActive(false);
                _enemyList[(int)oldFace][i].activateAI(false);
                //Debug.Log("Deactivating: " + _enemyList[(int)oldFace][i].gameObject.name);
            }
        }
    }

    //public function that will delete a give enemy from the array
    public void deleteFromList(GameObject delete) {
        //assuming the killed enemy is on the same face as the player
        _enemyList[(int)curFace].Remove(delete.GetComponent<BaseEnemy>());
    }

    //calculate the current face of the player so we know what enemies
    //to display
    private CubemapFace _getCurrentFace() {
        return vectorToCubemapFace(_playerGO.transform.up);
    }
    //calculate a given upaxis so it's relative cubeface can be calculated
    //NOTE: inputs must be read as exactly 1f or -1f on a single axis 
    public static CubemapFace vectorToCubemapFace(Vector3 thisUpAxis) {
        CubemapFace face = CubemapFace.Unknown;
        if (Mathf.Abs(thisUpAxis[0]) == 1f) {
            face = (thisUpAxis[0] > 0f) ? CubemapFace.PositiveX
                                        : CubemapFace.NegativeX;
        }
        else if (Mathf.Abs(thisUpAxis[1]) == 1f) {
            face = (thisUpAxis[1] > 0f) ? CubemapFace.PositiveY
                                        : CubemapFace.NegativeY;
        }
        else if (Mathf.Abs(thisUpAxis[2]) == 1f) {
            face = (thisUpAxis[2] > 0f) ? CubemapFace.PositiveZ
                                        : CubemapFace.NegativeZ;
        }
        return face;
    }

    public static void setCurrentFace(CubemapFace newFace) {
        
    }
}

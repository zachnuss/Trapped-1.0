using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of permutations and chooses which level to go when
/// - each scene has the permutation already in the, player data calls to this obj to choose which one we use and which ones we are not
/// 
/// NOTE: future iteration will incorperate a cube class that has a type, on construciton it will pick 6 random sides from its corresponding permutation and place them accordingly
/// 
/// PlayerData takes from here to choose the next level
/// </summary>
[CreateAssetMenu(fileName = "GameLevelData", menuName = "ScritableObjects/GameLevelData", order = 2)]
public class GameLevelData : ScriptableObject
{
    //future iteration will have these be walls instead of entire cubes
    [Header("Level 1 Permutations")]
    public GameObject[] level1Permutations;
    [Header("Level 2 Permutations")]
    public GameObject[] level2Permutations;
    [Header("Level 3 Permutations")]
    public GameObject[] level3Permutations;
    

    public GameObject ChooseLevelP(levelTypeE levelType)
    {
        int permutation;
        switch (levelType)
        {
            case levelTypeE.EasyLevel:
                permutation = Random.Range(0, level1Permutations.Length - 1);
                Debug.Log("Used p number: " + permutation);
                return level1Permutations[permutation];
            case levelTypeE.MidLevel:
                permutation = Random.Range(0, level2Permutations.Length - 1);
                Debug.Log("Used p number: " + permutation);
                return level2Permutations[permutation];
            case levelTypeE.Hardlevel:
                permutation = Random.Range(0, level3Permutations.Length - 1);
                Debug.Log("Used p number: " + permutation);
                return level3Permutations[permutation];
            default:
                Debug.Log("Null Permutation");
                return null;
        }
    }
}

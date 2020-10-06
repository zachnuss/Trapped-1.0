using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of permutations and chooses which level to go when
/// 
/// PlayerData takes from here to choose the next level
/// </summary>
[CreateAssetMenu(fileName = "GameLevelData", menuName = "ScritableObjects/GameLevelData", order = 2)]
public class GameLevelData : ScriptableObject
{
    public int check;
    
}

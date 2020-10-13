using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersistentData", menuName = "ScritableObjects/PersistentData", order = 3)]
public class SaveData : ScriptableObject
{
    private int highscore1;
    private int highscore2;
    private int highscore3;
    private int specialCoin;
    private int totalTimeAlive;
    private int totalEnemiesKilled;
    private int totalPowerupsCollected;
    private int totalCurrencyCollected;
    private int totalSpecialCoinsCollected;

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[System.Serializable]
public class PersistentData
{
    public int highScore1;
    public int highScore2;
    public int highScore3;
    public int specialCoin;
    public int totalTimeAlive;
    public int totalEnemiesKilled;
    public int totalPowerupsCollected;
    public int totalCurrencyCollected;
    public int totalSpecialCoinsCollected;
    public int materialChoice;

    

    public PersistentData(int highScore1Int, int highScore2Int, int highScore3Int, int specialCoinInt, int totalTimeAliveInt,
        int totalEnemiesKilledInt, int totalPowerupsCollectedInt, int totalCurrencyCollectedInt, int totalSpecialCoinsCollectedInt, int materialChoiceInt)
    {
        highScore1 = highScore1Int;
        highScore2 = highScore2Int;
        highScore3 = highScore3Int;
        specialCoin = specialCoinInt;
        totalTimeAlive = totalTimeAliveInt;
        totalEnemiesKilled = totalEnemiesKilledInt;
        totalPowerupsCollected = totalPowerupsCollectedInt;
        totalCurrencyCollected = totalCurrencyCollectedInt;
        totalSpecialCoinsCollected = totalSpecialCoinsCollectedInt;
        materialChoice = materialChoiceInt;
    }


    
}

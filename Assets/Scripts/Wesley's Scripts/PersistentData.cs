using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[System.Serializable]
public class PersistentData
{
    public int highScore1 = 0;
    public int highScore2 = 0;
    public int highScore3 = 0;
    public int specialCoin = 0;
    public float totalTimeSec = 0f;
    public float totalTimeMin = 0f;
    public float totalTimeHour = 0f;
    public int totalEnemiesKilled = 0;
    public int totalPowerupsCollected = 0;
    public int totalCurrencyCollected = 0;
    public int totalSpecialCoinsCollected = 0;
    public int materialChoice = 0;
    public bool characterChoice = false;

    

    public PersistentData(int highScore1Int, int highScore2Int, int highScore3Int, int specialCoinInt, float totalTimeSecFloat, float totalTimeMinFloat, float totalTimeHourFloat,
        int totalEnemiesKilledInt, int totalPowerupsCollectedInt, int totalCurrencyCollectedInt, int totalSpecialCoinsCollectedInt, int materialChoiceInt, bool characterChoiceBool)
    {
        highScore1 = highScore1Int;
        highScore2 = highScore2Int;
        highScore3 = highScore3Int;
        specialCoin = specialCoinInt;
        totalTimeSec = totalTimeSecFloat;
        totalTimeMin = totalTimeMinFloat;
        totalTimeHour = totalTimeHourFloat;
        totalEnemiesKilled = totalEnemiesKilledInt;
        totalPowerupsCollected = totalPowerupsCollectedInt;
        totalCurrencyCollected = totalCurrencyCollectedInt;
        totalSpecialCoinsCollected = totalSpecialCoinsCollectedInt;
        materialChoice = materialChoiceInt;
        characterChoice = characterChoiceBool;
    }


    
}

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
    public int totalRoombasKilled = 0;
    public int totalRegularsKilled = 0;
    public int totalShieldsKilled = 0;
    public int totalPowerupsCollected = 0;
    public int speedPowerupsCollected = 0;
    public int totalCurrencyCollected = 0;
    public int totalSpecialCoinsCollected = 0;
    public int materialChoice = 0;
    public int materialChoice2 = 0;
    public int maxLoops = 0;
    public int deathCounter = 0;
    public bool characterChoice = false;
    public bool character2Purchase = false;
    public bool character1Color2 = false;
    public bool character1Color3 = false;
    public bool character2Color2 = false;
    public bool character2Color3 = false;
    public bool achievementFirstTimer = false;
    public bool achievementVacuumMurderer = false;
    public bool achievementRevenge1 = false;
    public bool achievementRevenge2 = false;
    public bool achievementDoesItEnd = false;
    public bool achievementRedDead = false;
    public bool achievementFullWallet = false;
    public bool achievementRunner = false;
    public bool achievementJailBird = false;
    public bool achievementNoTrust = false;




    public PersistentData(int highScore1Int, int highScore2Int, int highScore3Int, int specialCoinInt, float totalTimeSecFloat, float totalTimeMinFloat, float totalTimeHourFloat,
        int totalEnemiesKilledInt, int totalPowerupsCollectedInt, int totalCurrencyCollectedInt, int totalSpecialCoinsCollectedInt, int materialChoiceInt, int materialChoice2Int, bool characterChoiceBool,
        bool character2PurchaseBool, bool character1Color2Bool, bool character1Color3Bool, bool character2Color2Bool, bool character2Color3Bool, bool achievementFirstTimerBool, bool achievementVacuumMurdererBool, 
        bool achievementRevenge1Bool, bool achievementRevenge2Bool, bool achievementDoesItEndBool, bool achievementRedDeadBool, bool achievementFullWalletBool, bool achievementRunnerBool, 
        bool achievementJailBirdBool, bool achievementNoTrustBool, int maxLoopsInt, int deathCounterInt, int totalRoombasKilledInt, int totalRegularsKilledInt, int totalShieldsKilledInt, int speedPowerupsCollectedInt)
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
        materialChoice2 = materialChoice2Int;
        characterChoice = characterChoiceBool;
        character2Purchase = character2PurchaseBool;
        character1Color2 = character1Color2Bool;
        character1Color3 = character1Color3Bool;
        character2Color2 = character2Color2Bool;
        character2Color3 = character2Color3Bool;
        achievementFirstTimer = achievementFirstTimerBool;
        achievementVacuumMurderer = achievementVacuumMurdererBool;
        achievementRevenge1 = achievementRevenge1Bool;
        achievementRevenge2 = achievementRevenge2Bool;
        achievementDoesItEnd = achievementDoesItEndBool;
        achievementRedDead = achievementRedDeadBool;
        achievementFullWallet = achievementFullWalletBool;
        achievementRunner = achievementRunnerBool;
        achievementJailBird = achievementJailBirdBool;
        achievementNoTrust = achievementNoTrustBool;
        maxLoops = maxLoopsInt;
        deathCounter = deathCounterInt;
        totalRoombasKilled = totalRoombasKilledInt;
        totalRegularsKilled = totalRoombasKilledInt;
        totalShieldsKilled = totalShieldsKilledInt;
        speedPowerupsCollected = totalPowerupsCollectedInt;
}


    
}

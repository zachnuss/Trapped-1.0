using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[CreateAssetMenu(fileName = "Data", menuName = "ScritableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Total Time")]
    public float timerSec;
    public float timerMin;
    public float timerHour;
    public float timerBetweenLevels;
    public float totalTime;

    [Header("Player Upgrade Stats")]
    public int healthUpgrade;
    public int damageUpgrade;
    public int speedUpgrade;

    [Header("Player base stats")]
    public int totalHealthBase;
    // public int totalSpeedBase;
    public int totalDamageBase;
    public float localHealth;

    [Header("Current level player is on: ZERO INDEXED")]
    public int OnLevel = 0;
    //0 = level 1 and so on

    [Header("Player score")]
    public int score = 0;
    public int matchScoreFromTime = 0;
    public int matchScoreFromEnemies = 0;
    public int matchEnemiesKilled = 0;
    public int matchPowerUpsCollected = 0;
    public int matchCurrencyCollected = 0;
    public int matchSpecialCoinCollected = 0;
    public int highScore1 = 0;
    public int highScore2 = 0;
    public int highScore3 = 0;

    [Header("Player Currency")]
    public int currency;
    public int specialCoins = 0;

    [Header("Player Currency")]
   // public Scene[] levels;
    public string[] levelsS;

    [Header("Prev and Next")]
    //public Scene nextLevel;
    //public Scene prevLevel;
    public string nextLevelStr;
    public string prevLevelStr;
    [Header("Levels Beaten")]
    public int levelsBeaten = 0;
    [Header("Game Level Data Obj")]
    public GameLevelData gameLevelData;

    [Header("Player Color")]
    public int materialChoice = 0;
    public int materialChoice2 = 0;
    public Material[] playerColor;
    public Material[] player2Color;
    public bool characterModelSwitch;

    //Player Persistent stats - Wesley
    private int totalEnemiesKilled = 0;
    private int totalPowerupsCollected = 0;
    private int totalCurrencyCollected = 0;
    private int totalSpecialCoinsCollected = 0;
    private int totalEnemyScore = 0;
    private float totalTimerSec;
    private float totalTimerMin;
    private float totalTimerHour;



    //called when level beat
    public void BeatLevel()
    {
        bool takeToModSelection = false;
        if (OnLevel <= levelsS.Length - 1)
        {
            timerBetweenLevels = timerSec;
            Debug.Log("Beat level");
            if (OnLevel > 0)
            {
                prevLevelStr = levelsS[OnLevel];        
            }
            
            OnLevel++;
            if (OnLevel > 2)
            {
                OnLevel = 0;
                //finished one loop of levels 1 - 3, now at reset we can add the thing for high risk/high reward and increase difficulty

                //take us to mod selection screen 
                takeToModSelection = true;

                //INCREASE DIFFICULTY HERE

            }

            if (OnLevel != levelsS.Length)
                nextLevelStr = levelsS[OnLevel];

            //goes 1 - 3 then back to one
            levelsBeaten++;

            //load store scene?
            if (!takeToModSelection)
                SceneManager.LoadScene("StoreScene");
            else
                SceneManager.LoadScene("GameLoopModSelection");
        }
    }

    //load next level
    public void LoadNextLevel()
    {
        if (OnLevel != levelsS.Length)
        {
            Debug.Log("Loading Next Level: " + levelsS[OnLevel]);
            timerSec += timerBetweenLevels;

            SceneManager.LoadScene(nextLevelStr);
        }
        else
            SceneManager.LoadScene(5);    
        //Debug.Log("LOAD END SCREEN HERE UWU");
    }

    //adds score
    public void AddScore(int addition)
    {
        score += addition;
    }

    //adds currency
    public void AddCurrency(int addition)
    {
        currency += addition;
        //curency adds score as well
        score += addition;
        TrackCurrencyGains(addition);
    }

    public void ResetUpgrades()
    {
        //when player starts at beginning, we reset the upgrades
        healthUpgrade = 0;
        damageUpgrade = 0;
        speedUpgrade = 0;
    }

    public void StartGame()
    {
        Debug.Log("Starting Game");
        ResetUpgrades();
        totalHealthBase = 100;
        //currently level 1 = scene 0
        OnLevel = 0;
        levelsBeaten = 0;
        //timer starts at 0
        timerHour = 0;
        timerSec = 0;
        timerMin = 0;
        timerBetweenLevels = 0;
        //currency starts at 0
        currency = 0;
        //score starts at 0
        score = 0; //added by wesley
        matchCurrencyCollected = 0;
        matchEnemiesKilled = 0;
        matchPowerUpsCollected = 0;
        matchScoreFromEnemies = 0;
        matchScoreFromTime = 0;
        matchSpecialCoinCollected = 0;
        localHealth = totalHealthBase;
        gameLevelData.InitialModSetup();
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Upgrades are bought in the store scene, they call these 
    /// 
    /// </summary>
    /// <param name="addition"></param>
    public void UpgradeHealth()
    {
        healthUpgrade++;
        totalHealthBase += 5;
        localHealth += 5;
        Debug.Log("Health Upgrade Purchased! New Health = " + localHealth + " out of " + totalHealthBase);
    }
    public void UpgradeDamage()
    {
        damageUpgrade++;
        Debug.Log("Damage Upgrade Purchased!");
    }
    public void UpgradeSpeed()
    {
        speedUpgrade++;
        Debug.Log("SPEED Upgrade Purchased!");
    }

    public void UpdateTime()
    {      
            if (timerMin >= 60)
            {
                timerHour++;
                timerMin = 0;
            }
        
        if (timerHour >= 99 && timerMin > 60 && timerSec > 60)
        {
            Debug.Log("you loose");
        }
    }

    //Wesley - update match values with several easy tricks! Also, they're private, so better to access through functions.
    public void TrackEnemyScore(int input)
    {
        matchScoreFromEnemies += input;
    }

    public void TrackTimeScore(int input)
    {
        matchScoreFromTime += input;
    }

    public void TrackCurrencyGains(int input)
    {
        matchCurrencyCollected += input;
    }

    public void TrackSpecialCoinGains(int input)
    {
        matchSpecialCoinCollected += input;
    }

    public void TrackEnemyKills(int input)
    {
        matchEnemiesKilled += input;
    }

    public void TrackPowerupGains(int input)
    {
        matchPowerUpsCollected += input;
    }

    //Turns character models on or off in menus
    public void SetCharacterChoiceMenu(UnityEngine.UI.Toggle choice)
    {
        GameObject[] character1 = new GameObject[8];
        for (int i = 0; i < character1.Length; i++)
        {
            character1[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
        }
        GameObject character2 = GameObject.Find("secondCharacter_low");
        if (choice.isOn == false)
        {
            characterModelSwitch = false;
            character2.GetComponent<MeshRenderer>().enabled = false;
            for (int i = 0; i < character1.Length; i++)
            {
                character1[i].GetComponent<SkinnedMeshRenderer>().enabled = true;
            }
            SetMenuColor(materialChoice);
        }
        else
        {
            characterModelSwitch = true;
            for (int i = 0; i < character1.Length; i++)
            {
                character1[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
            character2.GetComponent<MeshRenderer>().enabled = true;
            SetMenuColor(materialChoice2);
        }
    }

    //Sets character model on or off in game
    public void SetCharacterChoiceGame()
    {
        GameObject[] character1 = new GameObject[8];
        for (int i = 0; i < character1.Length; i++)
        {
            character1[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
        }
        GameObject character2 = GameObject.Find("secondCharacter_low");
        if (characterModelSwitch == false)
        {
            character2.GetComponent<MeshRenderer>().enabled = false;
            for (int i = 0; i < character1.Length; i++)
            {
                character1[i].GetComponent<SkinnedMeshRenderer>().enabled = true;
            }
            SetMenuColor(materialChoice);
        }
        else
        {
            for (int i = 0; i < character1.Length; i++)
            {
                character1[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
            character2.GetComponent<MeshRenderer>().enabled = true;
            SetMenuColor(materialChoice2);
        }
    }


    //Sets color choice, sets color on player - Wesley
    public void SetMenuColor(int input)
    {
        if (characterModelSwitch == false)
        {
            materialChoice = input;
        }
        if (characterModelSwitch == true)
        {
            materialChoice2 = input;
        }
        SetColor();
    }

    //Sets color on player - Wesley
    public void SetColor()
    {
        if (characterModelSwitch == false)
        {
            if (GameObject.Find("MainCharacter_Geo") == true)
            {
                GameObject[] character = new GameObject[8];
                for (int i = 0; i < character.Length; i++)
                {
                    character[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
                }
                for (int i = 0; i < character.Length; i++)
                {
                    character[i].GetComponent<SkinnedMeshRenderer>().material = playerColor[materialChoice];
                }
                //Debug.Log("This still happens");
            }
        }
        if(characterModelSwitch == true)
        {
            //Debug.Log("First step");
            if (GameObject.Find("secondCharacter_low") == true)
            {
                //Debug.Log("Second step");
                GameObject character;
                character = GameObject.Find("secondCharacter_low").gameObject;
                character.GetComponent<MeshRenderer>().material = player2Color[materialChoice2];
            }
        }
    }

    //Change persistent data tracking variables - Wesley
    public void AddSpecialCoins(int input)
    {
        specialCoins += input;
        TrackSpecialCoinGains(input);
    }

    public void UseSpecialCoin(int input)
    {
        specialCoins -= input;
    }

    public void AddTotalTimeAlive()
    {
        totalTimerHour += timerHour;
        totalTimerMin += timerMin;
        totalTimerSec += timerSec;
        totalTimerMin += Mathf.Floor(totalTimerSec / 60);
        totalTimerHour += Mathf.Floor(totalTimerMin / 60);
        totalTimerMin = totalTimerMin % 60;
        totalTimerSec = totalTimerSec % 60;
    }

    public void AddTotalEnemiesKilled(int input)
    {
        totalEnemiesKilled += input;
    }

    public void AddTotalPowerupsCollected(int input)
    {
        totalPowerupsCollected += input;
    }

    public void AddTotalCurrencyCollected(int input)
    {
        totalCurrencyCollected += input;
    }

    public void AddTotalSpecialCoinsCollected(int input)
    {
        totalSpecialCoinsCollected += input;
    }

    public void AddTotalEnemyScore(int input)
    {
        totalEnemyScore += input;
    }

    //Sets highscore values - Wesley
    public void SaveHighscore()
    {
        if (score > highScore1)
        {
            highScore3 = highScore2;
            highScore2 = highScore1;
            highScore1 = score;
        }
        else if (score > highScore2)
        {
            highScore3 = highScore2;
            highScore2 = score;
        }
        else if (score > highScore3)
        {
            highScore3 = score;
        }
    }

    //Endgame tasks - update all score values

    public void EndGameScoring()
    {
        SaveHighscore();
        AddTotalTimeAlive();
        AddTotalEnemiesKilled(matchEnemiesKilled);
        AddTotalPowerupsCollected(matchPowerUpsCollected);
        AddTotalCurrencyCollected(currency);
        AddTotalEnemyScore(matchScoreFromEnemies);
        SaveFile();
    }

    


    //Save Game - Wesley

    public void SaveFile()
    {
        Debug.Log("Saving Data");
        string destination = Application.persistentDataPath + "/TrappedSave.dat";
        FileStream file;

        file = File.Create(destination);

        PersistentData currentData = new PersistentData(highScore1, highScore2, highScore3, specialCoins, totalTimerSec, totalTimerMin, totalTimerHour,
            totalEnemiesKilled, totalPowerupsCollected, totalCurrencyCollected, totalSpecialCoinsCollected, materialChoice, materialChoice2, characterModelSwitch);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, currentData);
        file.Close();
    }

    public void LoadFile()
    {
        Debug.Log("Loading Data");

        string destination = Application.persistentDataPath + "/TrappedSave.dat";
        FileStream file;

        if (File.Exists(destination))
        {
            file = File.OpenRead(destination);
        }
        else
        {
            Debug.LogError("No save data");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        PersistentData loadData = (PersistentData)bf.Deserialize(file);
        highScore1 = loadData.highScore1;
        highScore2 = loadData.highScore2;
        highScore3 = loadData.highScore3;
        specialCoins = loadData.specialCoin;
        totalTimerSec = loadData.totalTimeSec;
        totalTimerMin = loadData.totalTimeMin;
        totalTimerHour = loadData.totalTimeHour;
        totalEnemiesKilled = loadData.totalEnemiesKilled;
        totalPowerupsCollected = loadData.totalPowerupsCollected;
        totalCurrencyCollected = loadData.totalCurrencyCollected;
        totalSpecialCoinsCollected = loadData.totalSpecialCoinsCollected;
        materialChoice = loadData.materialChoice;
        materialChoice2 = loadData.materialChoice2;
        characterModelSwitch = loadData.characterChoice;

        file.Close();
    }
}

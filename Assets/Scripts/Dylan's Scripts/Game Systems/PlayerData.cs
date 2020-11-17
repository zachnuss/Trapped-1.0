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
    private bool upgradePurchased = false; //Did the player purchase an upgrade this run?

    [Header("Player base stats")]
    public int totalHealthBase;
    // public int totalSpeedBase;
    public int totalDamageBase;
    public float localHealth;

    [Header("Current level player is on: ZERO INDEXED")]
    public int OnLevel = 0;
    [Header("Levels Beaten")]
    public int levelsBeaten = 0;
    private int loopCount = 0;
    //0 = level 1 and so on

    [Header("Player score")]
    public int score = 0;
    public int matchScoreFromTime = 0;
    public int matchScoreFromEnemies = 0;
    public int matchEnemiesKilled = 0;
    public int matchPowerUpsCollected = 0;
    public int speedPowerupsCollected = 0;
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

    [Header("Game Level Data Obj")]
    public GameLevelData gameLevelData;

    [Header("Player Color")]
    public int materialChoice = 0;
    public int materialChoice2 = 0;
    public int petChoice = 0;
    public Material[] playerColor;
    public Material[] player2Color;
    public bool characterModelSwitch;

    //Player Persistent stats - Wesley
    private int totalEnemiesKilled = 0;
    private int totalRoombasKilled = 0;
    private int totalRegularsKilled = 0;
    private int totalShieldsKilled = 0;
    private int totalPowerupsCollected = 0;
    private int totalCurrencyCollected = 0;
    private int totalSpecialCoinsCollected = 0;
    private int totalEnemyScore = 0;
    private int maxLoops = 0;
    private int deathCounter = 0;
    private float totalTimerSec;
    private float totalTimerMin;
    private float totalTimerHour;
    private bool characterChoice = false;
    private bool achievementFirstTimer = false;
    private bool achievementVacuumMurderer = false;
    private bool achievementRevenge1 = false;
    private bool achievementRevenge2 = false;
    private bool achievementDoesItEnd = false;
    private bool achievementRedDead = false;
    private bool achievementFullWallet = false;
    private bool achievementRunner = false;
    private bool achievementJailBird = false;
    private bool achievementNoTrust = false;

    [Header("Character Customization")]
    //Character Customization shopping - Wesley
    public bool character2Purchase = false;
    public bool characterPet2Purchase = false;
    public bool characterPet3Purchase = false;
    public bool character1Color2 = false;
    public bool character1Color3 = false;
    public bool character2Color2 = false;
    public bool character2Color3 = false;
    public int characterCost;
    public int colorCost;
    public int petCost;

    [HideInInspector]
    public bool godMode = false;
    [HideInInspector]
    public bool startAtHalf = false;

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-25-2020
    /// 
    /// Runs when leave is beat, sets next level/scene, updates varaibles
    /// </summary>
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
                loopCount++;
                if (loopCount >= 5)
                    achievementDoesItEnd = true;
                if (loopCount >= 2)
                    if (upgradePurchased == false)
                        achievementNoTrust = true;
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

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Loads next scene
    /// </summary>
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

    /// <summary>
    /// Wesley
    /// Updated: 10-20-2020
    /// 
    /// Adds score
    /// </summary>
    public void AddScore(int addition)
    {
        score += addition;
    }

    /// <summary>
    /// Wesley
    /// Updated: 10-20-2020
    /// 
    /// Adds to currency (and score based on currency)
    /// </summary>
    public void AddCurrency(int addition)
    {
        currency += addition;
        //curency adds score as well
        score += addition;
        TrackCurrencyGains(addition);
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// resets upgrades
    /// </summary>
    public void ResetUpgrades()
    {
        //when player starts at beginning, we reset the upgrades
        healthUpgrade = 0;
        damageUpgrade = 0;
        speedUpgrade = 0;
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-5-2020
    /// 
    /// Runs when game is initially started from menu, sets variables, loads level 1 and starts setting up the mods
    /// </summary>
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
        loopCount = 0;
        speedPowerupsCollected = 0;
        upgradePurchased = false;
        localHealth = totalHealthBase;
        gameLevelData.InitialModSetup();
        startAtHalf = false;
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-5-2020
    /// 
    /// Activated with gui, will reset values for testing and debugging, as if start game was activated
    /// </summary>
    public void ResetValues()
    {
        Debug.Log("Resetting Values");
        ResetUpgrades();
        totalHealthBase = 100;
        totalDamageBase = 20;
        //currently level 1 = scene 0
        OnLevel = 0;
        levelsBeaten = 0;
        //timer starts at 0
        timerHour = 0;
        timerSec = 0;
        timerMin = 0;
        timerBetweenLevels = 0;
        totalTime = 0;
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
        startAtHalf = false;
        gameLevelData.InitialModSetup();
    }

    /// <summary>
    /// Dylan loe
    /// Updated: 10-31-2020
    /// 
    /// Activates a god mode that can be toggled on and off before unity plays
    /// </summary>
    public void DebugMode()
    {
        if (godMode)
            godMode = false;
        else
            godMode = true;
    }

    /// <summary>
    /// Alexander & Dylan
    /// Updated: 10-20-2020
    /// 
    /// Upgrades health stat, as well as the local health of the player currently
    /// </summary>
    public void UpgradeHealth()
    {
        healthUpgrade++;
        totalHealthBase += 5;
        localHealth += 5;
        upgradePurchased = true;
        Debug.Log("Health Upgrade Purchased! New Health = " + localHealth + " out of " + totalHealthBase);
    }
    /// <summary>
    /// Alexander & Dylan
    /// Updated: 10-20-2020
    /// 
    /// Upgrades damage stat
    /// </summary>
    public void UpgradeDamage()
    {
        damageUpgrade++;
        upgradePurchased = true;
        Debug.Log("Damage Upgrade Purchased!");
    }
    /// <summary>
    /// Alexander & Dylan
    /// Updated: 10-20-2020
    /// 
    /// Upgrades speed stat
    /// </summary>
    public void UpgradeSpeed()
    {
        speedUpgrade++;
        upgradePurchased = true;
        Debug.Log("SPEED Upgrade Purchased!");
    }

    /// <summary>
    /// Dylan
    /// Updated: 10-20-2020
    /// 
    /// time variables, broguth from player
    /// </summary>
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

    /// <summary>
    /// Wesley
    /// Updated: 11-15-2020
    /// 
    /// update match values with several easy tricks! Also, they're private, so better to access through functions.
    /// Updated to manage achievements
    /// </summary>
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
        if (totalSpecialCoinsCollected >= 30)
            achievementFullWallet = true;
    }
    
    public void TrackEnemyKills(int input, GameObject enemy)
    {
        matchEnemiesKilled += input;
        if (enemy.gameObject.GetComponent<CommonGuard>() == true)
            totalRegularsKilled++;
        if (totalRegularsKilled > 10)
            achievementRevenge1 = true;
        if (enemy.gameObject.GetComponent<HallwayBot>() == true)
            totalRoombasKilled++;
        if (totalRoombasKilled > 20)
            achievementVacuumMurderer = true;
        if (enemy.gameObject.GetComponent<ShieldedGuard>() == true)
            totalShieldsKilled++;
        if (totalShieldsKilled > 10)
            achievementRevenge2 = true;
    }

    public void TrackPowerupGains(int input, powerUpType type)
    {
        matchPowerUpsCollected += input;
        if (type == powerUpType.speed)
            speedPowerupsCollected++;
        if (speedPowerupsCollected >= 5)
            achievementRunner = true;
    }

    /// <summary>
    /// Wesley
    /// Updated: 10-20-2020
    /// 
    /// Turns character models on or off in menus
    /// Also handles purchasing
    /// </summary>
    public void SetCharacterChoiceMenu()
    {
        characterModelSwitch = !characterModelSwitch;
        GameObject[] character1 = new GameObject[8];
        for (int i = 0; i < character1.Length; i++)
        {
            character1[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
        }
        GameObject character2 = GameObject.Find("secondCharacter_low");
        if (characterModelSwitch == false)
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
            if (character2Purchase == true)
            {
                characterModelSwitch = true;
                for (int i = 0; i < character1.Length; i++)
                {
                    character1[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                character2.GetComponent<MeshRenderer>().enabled = true;
                SetMenuColor(materialChoice2);
            }
            else
            {
                if (specialCoins >= characterCost)
                {
                    UseSpecialCoin(characterCost);
                    character2Purchase = true;
                    //update with a purchase
                    characterModelSwitch = true;
                    for (int i = 0; i < character1.Length; i++)
                    {
                        character1[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
                    }
                    character2.GetComponent<MeshRenderer>().enabled = true;
                    SetMenuColor(materialChoice2);
                }
                else
                {
                    characterModelSwitch = false;
                }
            }
        }
    }

    /// <summary>
    /// Wesley
    /// Updated: 10-20-2020
    /// 
    /// Sets character model on or off in game
    /// </summary>
    public void SetCharacterChoiceGame()
    {
        GameObject[] character1 = new GameObject[8];

        if (GameObject.Find("MainCharacter_Geo") == true)
        {
            for (int i = 0; i < character1.Length; i++)
            {
                character1[i] = GameObject.Find("MainCharacter_Geo").transform.GetChild(i).gameObject;
            }
        }
        GameObject character2 = null;
        if (GameObject.Find("secondCharacter_low") == true)
        {
            character2 = GameObject.Find("secondCharacter_low");
        }
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

    /// <summary>
    /// Wesley
    /// Updated: 10-20-2020
    /// 
    /// Sets color choice, sets color on player
    /// Also purchases colors using coins now
    /// </summary>
    public void SetMenuColor(int input)
    {
        if (characterModelSwitch == false)
        {
            if (input == 0)
            {
                materialChoice = input;
            }
            else if (input == 1 && character1Color3 == true)
            {
                materialChoice = input;
            }
            else if (input == 1 && character1Color3 == false)
            {
                if (specialCoins >= colorCost)
                {
                    UseSpecialCoin(colorCost);
                    character1Color3 = true;
                    materialChoice = input;
                }
            }
            else if (input == 2 && character1Color2 == true)
            {
                materialChoice = input;
            }
            else if (input == 2 && character1Color2 == false)
            {
                if (specialCoins >= colorCost)
                {
                    UseSpecialCoin(colorCost);
                    character1Color2 = true;
                    materialChoice = input;
                }
            }
        }
        if (characterModelSwitch == true)
        {
            if (input == 0)
            {
                materialChoice2 = input;
            }
            else if (input == 1 && character2Color2 == true)
            {
                materialChoice2 = input;
            }
            else if (input == 1 && character2Color2 == false)
            {
                if (specialCoins >= colorCost)
                {
                    UseSpecialCoin(colorCost);
                    character2Color2 = true;
                    materialChoice2 = input;
                }
            }
            else if (input == 2 && character2Color3 == true)
            {
                materialChoice2 = input;
            }
            else if (input == 2 && character2Color3 == false)
            {
                if (specialCoins >= colorCost)
                {
                    UseSpecialCoin(colorCost);
                    character2Color3 = true;
                    materialChoice2 = input;
                }
            }
        }
        SetColor();
    }

    /// <summary>
    /// Wesley
    /// Updated: 10-20-2020
    /// 
    /// Sets color on player
    /// </summary>
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

    /// <summary>
    /// Wesley
    /// Updated: 11-12-2020
    /// 
    /// Sets Pet on player
    /// Also controls purchasing of pets
    /// </summary>
    public void ChangePet(int input)
    {
        if(input == 0)
        {
            petChoice = input;
        }
        else if(input == 1 && characterPet2Purchase == true)
        {
            petChoice = input;
        }
        else if(input == 1 && characterPet2Purchase == false)
        {
            if (specialCoins >= petCost)
            {
                UseSpecialCoin(petCost);
                characterPet2Purchase = true;
                petChoice = input;
            }
        }
        else if (input == 2 && characterPet3Purchase == true)
        {
            petChoice = input;
        }
        else if (input == 2 && characterPet3Purchase == false)
        {
            if (specialCoins >= petCost)
            {
                UseSpecialCoin(petCost);
                characterPet3Purchase = true;
                petChoice = input;
            }
        }
        SetPet();
    }

    /// <summary>
    /// Wesley
    /// Updated: 11-12-2020
    /// 
    /// Sets Pet on player
    /// </summary>
    public void SetPet()
    {
        GameObject pet1 = GameObject.Find("pet_wasp");
        GameObject pet2 = GameObject.Find("Pet2");
        if(petChoice == 0)
        {
            pet1.GetComponent<Renderer>().enabled = false;
            pet2.GetComponent<Renderer>().enabled = false;
        }
        if(petChoice == 1)
        {
            pet2.GetComponent<Renderer>().enabled = false;
            pet1.GetComponent<Renderer>().enabled = true;
        }
        if(petChoice == 2)
        {
            pet1.GetComponent<Renderer>().enabled = false;
            pet2.GetComponent<Renderer>().enabled = true;
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
        if (timerHour >= 1 || timerMin >= 30)
            achievementFirstTimer = true;
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

    public void AddDeath()
    {
        deathCounter++;
        if (deathCounter >= 20)
            achievementJailBird = true;
    }

    public void GiveRedDead()
    {
        achievementRedDead = true;
    }

    //This section returns private variables to the persistent data script - Wesley
    //I guess I could have made properties but thats another set of variables, and lines setting them up
    public int ReturnTotalEnemiesKilled()
    {
        return totalEnemiesKilled;
    }

    public int ReturnTotalPowerUpsCollected()
    {
        return totalPowerupsCollected;
    }

    public int ReturnTotalCurrencyCollected()
    {
        return totalCurrencyCollected;
    }

    public int ReturnTotalEnemyValue()
    {
        return totalEnemyScore;
    }

    public int ReturnTotalSpecialCoins()
    {
        return totalSpecialCoinsCollected;
    }

    public string ReturnTotalTimePersistent()
    {
        string timeReadout;
        timeReadout = totalTimerHour + " Hours, " + totalTimerMin + " Minutes, and " + totalTimerSec + "Seconds.";
        return timeReadout;
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
            totalEnemiesKilled, totalPowerupsCollected, totalCurrencyCollected, totalSpecialCoinsCollected, materialChoice, materialChoice2,
            characterModelSwitch, character2Purchase, character1Color2, character1Color3, character2Color2, character2Color3, achievementFirstTimer, achievementVacuumMurderer,
            achievementRevenge1, achievementRevenge2, achievementDoesItEnd, achievementRedDead, achievementFullWallet, achievementRunner, achievementJailBird,
            achievementNoTrust, maxLoops, deathCounter, totalRoombasKilled, totalRegularsKilled, totalShieldsKilled, speedPowerupsCollected);
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
            Debug.Log("No save data");
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
        character2Purchase = loadData.character2Purchase;
        character1Color2 = loadData.character1Color2;
        character1Color3 = loadData.character1Color3;
        character2Color2 = loadData.character2Color2;
        character2Color3 = loadData.character2Color3;
        achievementFirstTimer = loadData.achievementFirstTimer;
        achievementVacuumMurderer = loadData.achievementVacuumMurderer;
        achievementRevenge1 = loadData.achievementRevenge1;
        achievementRevenge2 = loadData.achievementRevenge2;
        achievementDoesItEnd = loadData.achievementDoesItEnd;
        achievementRedDead = loadData.achievementRedDead;
        achievementFullWallet = loadData.achievementFullWallet;
        achievementRunner = loadData.achievementRunner;
        achievementJailBird = loadData.achievementJailBird;
        achievementNoTrust = loadData.achievementNoTrust;
        maxLoops = loadData.maxLoops;
        deathCounter = loadData.deathCounter;
        totalRoombasKilled = loadData.totalRoombasKilled;
        totalShieldsKilled = loadData.totalShieldsKilled;
        speedPowerupsCollected = loadData.speedPowerupsCollected;

        file.Close();
    }

    public void ActivateHalfHealthMod()
    {
        if(!startAtHalf)
        {
            //Debug.Log("cut health in half");
            startAtHalf = true;
            totalHealthBase = totalHealthBase / 2;
            localHealth = localHealth / 2;
        }
    }
}

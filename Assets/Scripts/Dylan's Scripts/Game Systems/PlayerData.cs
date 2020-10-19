﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

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
    public int matchScoreFromTime;
    public int matchScoreFromEnemies;
    public int matchEnemiesKilled;
    public int matchPowerUpsCollected;
    public int matchCurrencyCollected;
    public int matchSpecialCoinCollected;
    private int highScore1 = 0;
    private int highScore2 = 0;
    private int highScore3 = 0;

    [Header("Player Currency")]
    public int currency;
    public int specialCoins;

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
    public Material[] playerColor;

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

    //Wesley - update match values with several easy tricks!
    public void TrackEnemyScore(int input)
    {
        matchScoreFromEnemies += input;
    }

    public void TrackTimeScore(int input)
    {

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

    public void SetMenuColor(int input)
    {
        materialChoice = input;
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
        }
    }
}

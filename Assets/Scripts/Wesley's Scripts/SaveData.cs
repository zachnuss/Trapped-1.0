using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersistentData", menuName = "ScritableObjects/PersistentData", order = 3)]
[System.Serializable]
public class SaveData : ScriptableObject
{
    [SerializeField]
    //private SaveData _SaveData = new SaveData();

    private int highScore1 = 0;
    private int highScore2 = 0;
    private int highScore3 = 0;
    private int specialCoin = 0;
    private int totalTimeAlive = 0;
    private int totalEnemiesKilled = 0;
    private int totalPowerupsCollected = 0;
    private int totalCurrencyCollected = 0;
    private int totalSpecialCoinsCollected = 0;
    public PlayerData playerDataRef;

    private void Awake()
    {

    }

    public void AddSpecialCoin(int input)
    {
        specialCoin += input;
        AddTotalSpecialCoinsCollected(input);
    }

    public void UseSpecialCoin(int input)
    {
        specialCoin -= input;
    }

    public void AddTotalTimeAlive()
    {

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

    public void SaveHighscore()
    {
        if (playerDataRef.score > highScore1)
        {
            highScore3 = highScore2;
            highScore2 = highScore1;
            highScore1 = playerDataRef.score;
        }
        else if (playerDataRef.score > highScore2)
        {
            highScore3 = highScore2;
            highScore2 = playerDataRef.score;
        }
        else if (playerDataRef.score > highScore3)
        {
            highScore3 = playerDataRef.score;
        }
    }

    public void CreateSaveJson()
    {
        
        Debug.Log("Saving game");
        //string saveGame = JsonUtility.ToJson(_SaveData);
        //System.IO.File.WriteAllText(Application.persistentDataPath + "/SaveData.json", saveGame);
    }

    private void LoadSaveJson()
    {

    }
}

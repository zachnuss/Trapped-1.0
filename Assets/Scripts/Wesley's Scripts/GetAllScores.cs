using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAllScores : MonoBehaviour
{
    public PlayerData playerData;
    private Text ScoreTypes;

    // Start is called before the first frame update
    void Start()
    {
        ScoreTypes = gameObject.GetComponent<Text>();
        ScoreTypes.text = playerData.highScore1 + "\n" + playerData.highScore2 + "\n" + playerData.highScore3 + "\n" + playerData.score + "\n" + playerData.matchScoreFromTime 
            + "\n" + playerData.matchScoreFromEnemies + "\n" + playerData.matchEnemiesKilled + "\n" + playerData.matchPowerUpsCollected + "\n" + playerData.matchCurrencyCollected
            + "\n" + playerData.matchSpecialCoinCollected + "\n";
    }
}

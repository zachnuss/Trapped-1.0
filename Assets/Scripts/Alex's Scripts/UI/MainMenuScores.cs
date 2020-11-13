using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScores : MonoBehaviour
{
    //*THIS CODE WAS COPIED FROM WESLEY'S DISPLAYPERSISTANT CODE*

    //Variables to keep track of the scores for the main menu
    public PlayerData playerData;
    private Text ScoreTypes;

    // Start is called before the first frame update
    void Start()
    {
        ScoreTypes = gameObject.GetComponent<Text>();
        ScoreTypes.text = playerData.highScore1 + "\n" + playerData.highScore2 + "\n" + playerData.highScore3 + "\n";
    }
}
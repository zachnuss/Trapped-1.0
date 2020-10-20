using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScoreLabels : MonoBehaviour
{
    private Text ScoreTypes;

    // Start is called before the first frame update
    void Start()
    {
        ScoreTypes = gameObject.GetComponent<Text>();
        ScoreTypes.text = "Highscore 1: \n" + "Highscore 2: \n" + "Highscore 3: \n" + "Total Score: \n" + "Time Score: \n" 
           + "Defeated Enemy Value: \n" + "Enemies Killed: \n" + "Powerups Collected: \n" + "Currency Collected: \n"
           + "Special Currency Collected: \n";
    }
}

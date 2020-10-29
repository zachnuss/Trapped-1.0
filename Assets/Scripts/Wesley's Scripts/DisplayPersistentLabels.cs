using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPersistentLabels : MonoBehaviour
{
    private Text ScoreTypes;

    // Start is called before the first frame update
    void Start()
    {
        ScoreTypes = gameObject.GetComponent<Text>();
        ScoreTypes.text = "Highscore 1: \n" + "Highscore 2: \n" + "Highscore 3: \n" + "Enemies Killed: \n" + "Defeated Enemy Value: \n"
           + "Power Ups Collected: \n" + "Total Special Coins: \n" + "Total Time Alive: \n";
    }
}

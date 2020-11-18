using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SceneLoading : MonoBehaviour
{
    [SerializeField]
    public Image progressBar;

    public PlayerData myPlayerData;

    // Start is called before the first frame update
    void Start()
    {
        //start async op
        StartCoroutine(LoadAsyncOp());
    }

    IEnumerator LoadAsyncOp()
    {
        //create an async op
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(myPlayerData.nextLevelStr);
        //update bar based on asynch op progress
        while(gameLevel.progress < 1)
        {
            progressBar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }

        //when done, we load target game scene


       // yield return new WaitForEndOfFrame();
    }
}

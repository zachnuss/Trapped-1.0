using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Loading Scenes
/// </summary>
public class SceneLoading : MonoBehaviour
{
    [SerializeField]
    public Image progressBar;

    public PlayerData myPlayerData;

    void Start()
    {
        //start async op
        StartCoroutine(LoadAsyncOp());
    }

    /// <summary>
    /// Dylan Loe
    /// Updated 11 2020
    /// 
    /// Gets progress for running async op for loading next level. Displays on a progress bar in UI
    /// </summary>
    IEnumerator LoadAsyncOp()
    {
        //create an async op
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(myPlayerData.nextSceneStr);
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

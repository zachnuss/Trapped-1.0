using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Future iteration will use a cube class to create the sides and set that up instead of chooseing what cube we use we choose what sides on said cube
/// </summary>
public enum levelTypeE {
    EasyLevel,
    MidLevel,
    Hardlevel
};

public class LevelSetup : MonoBehaviour
{
    public GameLevelData gameLevelData;

    public levelTypeE type;

    public GameObject permutation;

    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        SetPlayer();
        permutation = gameLevelData.ChooseLevelP(type);
        Instantiate(permutation);
        Debug.Log(permutation.name);
    }

    void SetPlayer()
    {
        switch (type)
        {
            case levelTypeE.EasyLevel:
                _player.transform.position = new Vector3(0.45f, 23.01f, 0.27f);
                break;
            case levelTypeE.MidLevel:
                //_player.transform.position = new Vector3(0, 23.998f, 0);
                break;
            case levelTypeE.Hardlevel:
                _player.transform.position = new Vector3(0, 38.1f, 0);
                break;
            default:
                break;
        }
    }
}

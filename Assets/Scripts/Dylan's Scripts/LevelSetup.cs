using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum levelTypeE {
    EasyLevel,
    MidLevel,
    Hardlevel
};

public class LevelSetup : MonoBehaviour
{
    GameLevelData gameLevelData;

    public levelTypeE type;

    private void Awake()
    {
        
    }
}

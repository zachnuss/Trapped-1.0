﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public PlayerData playerData;
    void Start()
    {
        playerData.LoadFile();
    }
}

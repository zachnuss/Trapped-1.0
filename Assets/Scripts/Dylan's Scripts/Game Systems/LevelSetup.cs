﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Future iteration will use a cube class to create the sides and set that up instead of chooseing what cube we use we choose what sides on said cube
/// </summary>
public enum levelTypeE {
    EasyLevel,
    MidLevel,
    Hardlevel,
    none
};


public class LevelSetup : MonoBehaviour
{
    //player can overide
    //[HideInInspector]
    
    [HideInInspector]
    public int permutationNum = 0;
    public GameLevelData gameLevelData;

    public levelTypeE type;

    public GameObject permutation;

    private GameObject _player;

    public Modifier[] currentModsInLevel;

    [SerializeField, HideInInspector]
    public bool dontLoadPermutation;
    [SerializeField, HideInInspector]
    public bool overrideRandomLevel;

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-3-2020
    /// 
    /// Sets variables, modifiers and brings in the level permutation to load
    /// </summary>
    private void Awake()
    {
       

        _player = GameObject.FindGameObjectWithTag("Player");
        SetPlayer();
        //set modifiers
        SetModifiers();
        if (!dontLoadPermutation)
        {
            if (overrideRandomLevel)
            {
                Debug.Log("here");
                permutation = gameLevelData.ChooseLevelP(type);
                Instantiate(permutation);
                Debug.Log(permutation.name);
            }
            else
            {
                permutation = gameLevelData.GetPermutation(type, permutationNum);
                //if(permutation != null)
                Instantiate(permutation);
                Debug.Log("Manual Override for Permutation: " + permutation.name);
            }
        }
        
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// Sets player to starting point beased on what level is loaded
    /// </summary>
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

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// if player choose mods at end of loop they go into each level here
    /// </summary>
    void SetModifiers()
    {
        currentModsInLevel = gameLevelData.mods;
        ActivateModifiers();
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 10-20-2020
    /// 
    /// physicall activate mods in level
    /// </summary>
    void ActivateModifiers()
    {
        for (int modIndex = 0; modIndex < currentModsInLevel.Length; modIndex++)
        {
            if (currentModsInLevel[modIndex].modType == modifierType.shields_and_regainMOD && currentModsInLevel[modIndex].modActive)
            {
                //turn on health regen bool on player
                _player.GetComponent<PlayerMovement>().healthRegen = true;
            }
            if(currentModsInLevel[modIndex].modType == modifierType.doubleDamageMOD && currentModsInLevel[modIndex].modActive)
            {
                _player.GetComponent<PlayerMovement>().doubleScoreMod = true;
            }

            if(currentModsInLevel[modIndex].modType == modifierType.SerratedAmmunition && currentModsInLevel[modIndex].modActive)
            {
                _player.GetComponent<PlayerMovement>().serratedMod = true;
            }
            if(currentModsInLevel[modIndex].modType == modifierType.AdvancedSimulant && currentModsInLevel[modIndex].modActive)
            {
                _player.GetComponent<PlayerMovement>().doubleDamage = true;
            }
            if(currentModsInLevel[modIndex].modType == modifierType.LowPoweredGeneratorMOD && currentModsInLevel[modIndex].modActive)
            {
               // Debug.Log("health mod");
                _player.GetComponent<PlayerMovement>().personalSheild = true;
                _player.GetComponent<PlayerMovement>().sheildObj.SetActive(true);

                _player.GetComponent<PlayerMovement>().playerData.ActivateHalfHealthMod();
            }
            if(currentModsInLevel[modIndex].modType == modifierType.TrackingAmmunitionMOD && currentModsInLevel[modIndex].modActive)
            {
                _player.GetComponent<PlayerMovement>().trackingAmmunitionMod = true;
                _player.GetComponent<PlayerMovement>().mortarGrid.SetActive(true);
                //FindObjectOfType<MortarModBehavior>().enabled = true;
                //GameObject.Find("MortarGrid").SetActive(true);

            }
        }
    
           
        Debug.Log("Mods Now Active in Level");
    }
}

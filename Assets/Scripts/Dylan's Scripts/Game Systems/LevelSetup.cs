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

    public Modifier[] currentModsInLevel;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        SetPlayer();
        permutation = gameLevelData.ChooseLevelP(type);
        Instantiate(permutation);
        Debug.Log(permutation.name);

        //set modifiers
        SetModifiers();
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

    //if player choose mods at end of loop they go into each level here
    void SetModifiers()
    {
        currentModsInLevel = gameLevelData.mods;
        ActivateModifiers();
    }


    //physicall activate mods in level
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
        }

        Debug.Log("Mods Now Active in Level");
    }
}

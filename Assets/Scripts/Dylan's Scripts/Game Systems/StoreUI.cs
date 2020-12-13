using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dylan Loe
/// Updated: 10 2020
/// 
/// Manage store UI Buttons. Each button runs one of these functions. Simply runs functions from PlayerData
/// </summary>

public class StoreUI : MonoBehaviour
{
    public PlayerData playerData;

    //when player is ready to go to next level
    public void OnNextLevelButton()
    {
        playerData.LoadNextLevel();
    }

    //when player buys an upgrade
    public void OnDamageUpgradeButton()
    {
        playerData.UpgradeDamage();
    }

    public void OnHealthUpgradeButton()
    {
        playerData.UpgradeHealth();
    }

    public void OnSpeedUpgradeButton()
    {
        playerData.UpgradeSpeed();
    }
}

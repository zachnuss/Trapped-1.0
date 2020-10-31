using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerData))]
public class GameDataInspectorEditor : Editor
{
    /// <summary>
    /// Dylan Loe
    /// Updated 10-30-2020
    /// 
    /// Adds more functionality to PlayerData scriptable obj
    /// </summary>
    public override void OnInspectorGUI()
    {
        PlayerData myPData = (PlayerData)target;
        
        if (GUILayout.Button("Reset to default values"))
        {
            myPData.ResetValues();
        }
        //will add a debug mode that adds a godmode that can be toggled on and off from here

        base.OnInspectorGUI();
        
    }
}

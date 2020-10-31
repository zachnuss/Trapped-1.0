using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerData))]
public class GameDataInspectorEditor : Editor
{
    private string debugMode;
    /// <summary>
    /// Dylan Loe
    /// Updated 10-31-2020
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

        if(GUILayout.Button("Debug Mode"))
        {
            myPData.DebugMode();
            
           // if(myPData.godMode)
           
        }
        if (myPData.godMode)
            debugMode = "Active";
        else
            debugMode = "Inactive";

        EditorGUILayout.LabelField("Debug Mode: ", debugMode);
        base.OnInspectorGUI();
        
    }
}

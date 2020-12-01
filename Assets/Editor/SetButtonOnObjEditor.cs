using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SetButtonOnObj))]
public class SetButtonOnObjEditor : Editor
{
    string spawnNothingStr;
    SetButtonOnObj mySetButtonOnObj;

    public void Awake()
    {
        mySetButtonOnObj = (SetButtonOnObj)target;
    }
    // bool spawnNothing;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("This Prefab must be unpacked completely when in your scene.");
        EditorGUILayout.Space();

        if (GUILayout.Button("Toggle Replacement Asset"))
        {
            if (mySetButtonOnObj.spawnNothing)
            {
                // spawnNothingStr = "Will NOT Spawn Replacement Asset";
                mySetButtonOnObj.spawnNothing = false;
            }
            else
            {
                mySetButtonOnObj.spawnNothing = true;
                //spawnNothingStr = "WILL Spawn Replacement Asset";
            }
        }

        if (mySetButtonOnObj.spawnNothing)
            spawnNothingStr = "Will NOT Spawn Replacement Asset";
        else
            spawnNothingStr = "WILL Spawn Replacement Asset";

        EditorGUILayout.LabelField(spawnNothingStr);
        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}

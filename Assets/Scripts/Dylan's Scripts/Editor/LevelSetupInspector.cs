using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSetup))]
public class LevelSetupInspector : Editor
{
    string overrideStatus;
    LevelSetup myLevelData;

    private void Awake()
    {
        myLevelData = (LevelSetup)target;
    }

    /// <summary>
    /// Dylan Loe
    /// 11-3-2020
    /// 
    /// Added debug override for levelsetup script
    /// </summary>
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("This Prefab must be unpacked completely when in your scene.");
        EditorGUILayout.Space();

        string buttonStr;
        if (!myLevelData.dontLoadPermutation)
            buttonStr = "Permutation on Start";
        else
            buttonStr = "No Permutation on Start";


        if (GUILayout.Button("Manual Permutation Override"))
        {
            //Debug.Log("push");
            if (myLevelData.overrideRandomLevel)
                myLevelData.overrideRandomLevel = false;
            else
                myLevelData.overrideRandomLevel = true;
        }

        EditorGUILayout.Space();
        if (myLevelData.overrideRandomLevel)
        {
            overrideStatus = "Override Active";
            if(!myLevelData.dontLoadPermutation)
                EditorGUILayout.IntField("Permutation to Override: ", myLevelData.permutationNum);

            if (GUILayout.Button(buttonStr))
            {
                if (!myLevelData.dontLoadPermutation)
                {
                   // buttonStr = "No Permutation on Start";
                    myLevelData.dontLoadPermutation = true;
                    EditorGUILayout.HelpBox("No Permutation to Load on Start.", MessageType.Warning);
                }
                else
                {
                    //EditorGUILayout.HelpBox("Permutation to Load on Start.", MessageType.Warning);
                    myLevelData.dontLoadPermutation = false;
                }
            }

            
        }
        else
            overrideStatus = "Override Inctive";

        EditorGUILayout.LabelField("Override Status: ", overrideStatus);
        

        EditorGUILayout.Space();
        base.OnInspectorGUI();
    }
}

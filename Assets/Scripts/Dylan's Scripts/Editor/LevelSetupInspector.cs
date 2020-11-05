using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSetup))]
public class LevelSetupInspector : Editor
{
    string overrideStatus;
    /// <summary>
    /// Dylan Loe
    /// 11-3-2020
    /// 
    /// Added debug override for levelsetup script
    /// </summary>
    public override void OnInspectorGUI()
    {
        LevelSetup myLevelData = (LevelSetup)target;

        if(GUILayout.Button("Manual Permutation Override"))
        {
            //Debug.Log("push");
            if (myLevelData.gameLevelData.overrideRandomLevel)
                myLevelData.gameLevelData.overrideRandomLevel = false;
            else
                myLevelData.gameLevelData.overrideRandomLevel = true;
        }

        if (myLevelData.gameLevelData.overrideRandomLevel)
        {
            overrideStatus = "Override Active";
            EditorGUILayout.IntField("Permutation to Override: ", myLevelData.permutationNum);

           // if (GUILayout.Button("No Permutation on Start"))
         //   {
           //     if (!myLevelData.dontLoadPermutation)
            //    {
            //        myLevelData.dontLoadPermutation = true;
            //        EditorGUILayout.HelpBox("No Permutation to Load on Start.", MessageType.Warning);
             //   }
             //   else
             //   {
             //       myLevelData.dontLoadPermutation = false;
             //   }
           // }

            
        }
        else
            overrideStatus = "Override Inctive";

        EditorGUILayout.LabelField("Override Status: ", overrideStatus);
        

        EditorGUILayout.Space();
        base.OnInspectorGUI();
    }
}

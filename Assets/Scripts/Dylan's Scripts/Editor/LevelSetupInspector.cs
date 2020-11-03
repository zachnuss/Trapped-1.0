using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSetup))]
public class LevelSetupInspector : Editor
{
    string overrideStatus;
    public override void OnInspectorGUI()
    {
        LevelSetup myLevelData = (LevelSetup)target;

        if(GUILayout.Button("Manual Permutation Override"))
        {
            if (myLevelData.overrideRandomLevel)
                myLevelData.overrideRandomLevel = false;
            else
                myLevelData.overrideRandomLevel = true;
        }

        if (myLevelData.overrideRandomLevel)
        {
            overrideStatus = "Override Active";
            EditorGUILayout.IntField("Permutation to Override: ", myLevelData.permutationNum);
        }
        else
            overrideStatus = "Override Inctive";

        EditorGUILayout.LabelField("Override Status: ", overrideStatus);

        EditorGUILayout.Space();
        base.OnInspectorGUI();
    }
}

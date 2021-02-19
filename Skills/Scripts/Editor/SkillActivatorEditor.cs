using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillActivator), true)]
public class SkillActivatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myTarget = (SkillActivator)target;
        if (GUILayout.Button("Active First"))
        {
            myTarget.FirstState();
        }

        if (GUILayout.Button("Active Second"))
        {
            myTarget.SecondState();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterRagDollActivate))]
public class CharacterRagDollEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myTarget = (CharacterRagDollActivate)target;
        if (GUILayout.Button("Enable"))
        {
            myTarget.ActiveRagDoll(true);
        }

        if (GUILayout.Button("Disable"))
        {
            myTarget.ActiveRagDoll(false);
        }
    }
}

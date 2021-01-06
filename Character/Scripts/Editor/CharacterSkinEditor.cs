using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterSkin))]
public class CharacterSkinEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myTarget = (CharacterSkin)target;

        if (GUILayout.Button("Set team 1 skin"))
        {
            myTarget.SetMaterial(1);
        }

        if (GUILayout.Button("Set team 2 skin"))
        {
            myTarget.SetMaterial(2);
        }
    }
}

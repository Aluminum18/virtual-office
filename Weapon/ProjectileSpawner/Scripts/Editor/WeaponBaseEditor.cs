using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseWeapon), true)]
public class WeaponBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myTarget = (BaseWeapon)target;
        if (GUILayout.Button("Start Attack"))
        {
            if (Application.isPlaying)
            {
                myTarget.StartAttack();
            }
        }
    }
}

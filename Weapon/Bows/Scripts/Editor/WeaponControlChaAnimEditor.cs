using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponControlCharacterAnim), true)]
public class WeaponControlChaAnimEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myTarget = (WeaponControlCharacterAnim)target;
        if (GUILayout.Button("Active"))
        {
            myTarget.ActiveWeaponLayer(true);
        }

        if (GUILayout.Button("Inactive"))
        {
            myTarget.ActiveWeaponLayer(false);
        }
    }
}

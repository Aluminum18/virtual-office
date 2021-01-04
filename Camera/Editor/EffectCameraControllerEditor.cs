using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectCameraController))]
public class EffectCameraControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myTarget = (EffectCameraController)target;

        if (GUILayout.Button("Enable E-vision"))
        {
            myTarget.ActiveXray(true);
        }
        if (GUILayout.Button("Disable E-vision"))
        {
            myTarget.ActiveXray(false);
        }
    }
}

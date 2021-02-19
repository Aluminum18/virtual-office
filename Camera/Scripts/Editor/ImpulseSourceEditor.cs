using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ImpulseSource))]
public class ImpulseSourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myTarget = (ImpulseSource)target;

        if (GUILayout.Button("Gen Impulse"))
        {
            myTarget.GenerateImpulse();
        }
    }
}

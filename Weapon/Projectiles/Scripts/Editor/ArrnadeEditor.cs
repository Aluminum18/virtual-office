using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArrNade))]
public class ArrNadeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myTarget = (ArrNade)target;
        if (GUILayout.Button("Explode"))
        {
            if (Application.isPlaying)
            {
                myTarget.IsExploded = false;
                myTarget.Explose();
            }
        }
    }
}

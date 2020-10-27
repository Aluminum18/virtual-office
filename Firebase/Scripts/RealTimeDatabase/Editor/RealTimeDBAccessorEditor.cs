using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RealTimeDBAccessor))]
public class RealTimeDBAccessorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RealTimeDBAccessor myTarget = (RealTimeDBAccessor)target;

        if (GUILayout.Button("GetData"))
        {
            myTarget.GetData(myTarget.key, result =>
            {
                myTarget.response = result;
            });
        }
    }
}



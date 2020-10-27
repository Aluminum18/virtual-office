using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FollowingCamera))]
public class FollowingCameraEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myTarget = (FollowingCamera)target;

        Transform targetTransform = myTarget.TranformToFollow;
        if (targetTransform != null)
        {
            myTarget._offset = myTarget.transform.position - targetTransform.position;
        }
        DrawDefaultInspector();
    }
}

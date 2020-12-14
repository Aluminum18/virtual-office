using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterAttribute))]
public class CharacterAttEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var myTarget = (CharacterAttribute)target;
        if (GUILayout.Button("Assign test input (player 1)"))
        {
            myTarget.GetComponent<CharacterMoving>().CheckAndSubcribeInput();
            myTarget.GetComponent<CharacterRotating>().CheckAndSubcribeInput();
            myTarget.GetComponent<CharacterAction>().SubcribeInput();
            myTarget.AnimController.SubcribeInput();
        }
    }
}

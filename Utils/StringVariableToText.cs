using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StringVariableToText : MonoBehaviour
{
    [SerializeField]
    private StringVariable _stringVariable;
    [SerializeField]
    private TMP_Text _textMesh;

    private void OnEnable()
    {
        _stringVariable.OnValueChange += UpdateStringValue;
    }

    private void OnDisable()
    {
        _stringVariable.OnValueChange -= UpdateStringValue;
    }

    private void UpdateStringValue(string newValue)
    {
        if (newValue.Equals(_textMesh.text))
        {
            return;
        }

        _textMesh.text = newValue;
    }
}

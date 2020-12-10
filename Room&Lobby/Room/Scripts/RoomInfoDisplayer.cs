using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomInfoDisplayer : MonoBehaviour
{
    [SerializeField]
    private RoomOptionsSO _roomOptions;

    [SerializeField]
    private TMP_Text _roomName;

    private void Start()
    {
        _roomName.text = _roomOptions.RoomName;
    }
}

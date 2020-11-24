using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateRoomOptionsPopup : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private RoomSO _roomOpstions;

    [Header("Config")]
    [SerializeField]
    private TMP_InputField _roomNameIF;

    public void SetRoomName()
    {
        _roomOpstions.RoomName = _roomNameIF.text;
    }
}

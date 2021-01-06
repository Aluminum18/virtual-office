using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JoinRoomPopup : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private RoomOptionsSO _roomOptions;

    [Header("Config")]
    [SerializeField]
    private TMP_InputField _roomName;
    [SerializeField]
    private Button _joinButton;

    public void UpdateJoinButtonStatus()
    {
        if (string.IsNullOrEmpty(_roomName.text))
        {
            _joinButton.interactable = false;
            return;
        }

        _joinButton.interactable = true;
    }

    public void SetJoinRoomName()
    {
        if (string.IsNullOrEmpty(_roomName.text))
        {
            return;
        }

        _roomOptions.RoomName = _roomName.text;
    }

    private void Start()
    {
        UpdateJoinButtonStatus();
    }
}

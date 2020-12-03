using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateRoomOptionsPopup : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private RoomOptionsSO _roomOptions;

    [Header("Config")]
    [SerializeField]
    private TMP_InputField _roomNameIF;
    [SerializeField]
    private Button _createButton;

    public void SetRoomName()
    {
        _roomOptions.RoomName = _roomNameIF.text;
    }

    public void UpdateCreateButton()
    {
        if (string.IsNullOrEmpty(_roomNameIF.text))
        {
            _createButton.interactable = false;
            return;
        }

        _createButton.interactable = true;
    }

    private void Start()
    {
        UpdateCreateButton();
    }
}

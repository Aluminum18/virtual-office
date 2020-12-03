using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInRoomDisplayer : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private RoomInfoSO _roomInfo;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onRoomInfoSOChange;

    [Header("Config")]
    [SerializeField]
    private int _position;
    [SerializeField]
    private TMP_Text _playerName;

    public void SetPlayerName(params object[] args)
    {
        string _playerId = _roomInfo.GetPlayerIdAtPos(_position);
        if (string.IsNullOrEmpty(_playerId))
        {
            _playerName.text = "Open";
            return;
        }

        UserInfoFetcher.Instance.GetUserInfo(_playerId,
            userInfo =>
            {
                _playerName.text = userInfo.displayName;
            },
            () =>
            {

            });

    }

    private void OnEnable()
    {
        SetPlayerName();
        _onRoomInfoSOChange.Subcribe(SetPlayerName);
    }

    private void OnDisable()
    {
        _onRoomInfoSOChange.Unsubcribe(SetPlayerName);
    }
}

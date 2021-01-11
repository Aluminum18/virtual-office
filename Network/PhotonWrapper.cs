using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonWrapper : MonoBehaviourPunCallbacks
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _version;
    [SerializeField]
    private RoomOptionsSO _roomOptions;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onRoomCreated;
    [SerializeField]
    private GameEvent _onJoinedRoom;
    [SerializeField]
    private GameEvent _onLeftRoom;

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            return;
        }
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = _version.Value;
    }

    public void CreateRoom()
    {
        var roomOption = new RoomOptions()
        {
            MaxPlayers = _roomOptions.MaxPlayersPerTeam,
            IsVisible = _roomOptions.IsVisible
        };

        if (string.IsNullOrEmpty(_roomOptions.RoomName))
        {
            _roomOptions.RoomName = System.DateTime.Now.Ticks.ToString();
        }
        PhotonNetwork.CreateRoom(_roomOptions.RoomName, roomOption);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void JoinDefaultRoom()
    {
        if (string.IsNullOrEmpty(_roomOptions.RoomName))
        {
            Debug.LogError("Room name is empty");
            return;
        }
        PhotonNetwork.JoinRoom(_roomOptions.RoomName);
    }

    public override void OnCreatedRoom()
    {
        _onRoomCreated?.Raise();
        Debug.Log("Room Created!");
    }

    public override void OnJoinedRoom()
    {
        _onJoinedRoom?.Raise();
    }

    public override void OnLeftRoom()
    {
        _onLeftRoom?.Raise();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"fail to join room error code: [{returnCode}] message: [{message}]");
    }

    private void Awake()
    {
        Connect();
    }

    private void OnApplicationQuit()
    {
        CheckStatusAndLeaveRoomIfNeeded();
    }

    private void CheckStatusAndLeaveRoomIfNeeded()
    {
        if (!PhotonNetwork.InRoom)
        {
            return;
        }

        PhotonNetwork.LeaveRoom();
    }
}

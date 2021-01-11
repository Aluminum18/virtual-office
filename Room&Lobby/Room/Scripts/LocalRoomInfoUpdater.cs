using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalRoomInfoUpdater : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private RoomOptionsSO _roomOption;
    [SerializeField]
    private RoomInfoAccessor _roomInfoDBAccessor;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onCreatedRoom;
    [SerializeField]
    private GameEvent _onJoinedRoom;
    [SerializeField]
    private GameEvent _onLeftRoom;
    [SerializeField]
    private GameEvent _onRoomPlayerDataChangedFromDb;

    private void UpdateCreatedRoomInfo(params object[] args)
    {
        //_roomInfo.CreateNewRoomInfo(true);
    }

    private void UpdateJoinedRoomInfo(params object[] args)
    {
        _roomInfoDBAccessor.GetRoomInfo(_roomOption.RoomName,
            roomData =>
            {
                //_roomInfo.CreateInfoByDbData(roomData);
                //_roomInfo.AddToAvailableSlot(_userId.Value, true);
            });
    }

    private void DropPlayerWhenLeft(params object[] args)
    {
        //_roomInfo.DropPlayer(_userId.Value);
    }

    private void UpdateRoomDataFromDb(params object[] args)
    {
        var changedRoomData = (FireStoreRoomData)args[0];
        //_roomInfo.CreateInfoByDbData(changedRoomData);
    }

    private void OnEnable()
    {
        _onCreatedRoom.Subcribe(UpdateCreatedRoomInfo);
        _onJoinedRoom.Subcribe(UpdateJoinedRoomInfo);
        _onLeftRoom.Subcribe(DropPlayerWhenLeft);
        _onRoomPlayerDataChangedFromDb.Subcribe(UpdateRoomDataFromDb);
    }

    private void OnDisable()
    {
        _onCreatedRoom.Unsubcribe(UpdateCreatedRoomInfo);
        _onJoinedRoom.Unsubcribe(UpdateJoinedRoomInfo);
        _onLeftRoom.Unsubcribe(DropPlayerWhenLeft);
        _onRoomPlayerDataChangedFromDb.Unsubcribe(UpdateRoomDataFromDb);
    }
}

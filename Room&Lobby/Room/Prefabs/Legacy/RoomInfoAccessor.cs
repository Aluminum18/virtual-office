using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Text;
using Firebase.Database;

public class RoomInfoAccessor : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private RoomOptionsSO _roomOptions;
    [SerializeField]
    private RoomInfoSO _roomInfo;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onRoomPlayerDataChangedFromLocal;
    [SerializeField]
    private GameEvent _onPlayerReady;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onRoomPlayerDataChangedFromDb;

    [Header("Config")]
    [SerializeField]
    private RealTimeDBAccessor _dbAccessor;

    private ListenerRegistration _roomInfoChangeListener;
    private string _currentRoomName = "";

    private StringBuilder _sb = new StringBuilder();

    public void GetRoomInfo(string roomName, Action<FireStoreRoomData> callback)
    {
        _sb.Clear();
        _sb.Append(FireStoreCollection.MULTIPLAYER_ROOM).Append("/").Append(_roomOptions.RoomName);
        var roomRef = FirebaseFirestore.DefaultInstance.Document(_sb.ToString());

        roomRef.GetSnapshotAsync().ContinueWithOnMainThread(
            getRoomInfoTask =>
            {
                if (getRoomInfoTask.IsFaulted)
                {
                    Debug.LogError($"Cannot get room info with name [{roomName}]");
                    return;
                }

                var roomData = getRoomInfoTask.Result.ConvertTo<FireStoreRoomData>();
                callback?.Invoke(roomData);
            });
    }

    public void UpdateRoomPlayerInfoToDB(params object[] args)
    {
        var newRoomInfo = new FireStoreRoomData();
        newRoomInfo.roomName = _roomOptions.RoomName;
        newRoomInfo.maxPlayerPerTeam = _roomOptions.MaxPlayersPerTeam;
        newRoomInfo.team1 = _roomInfo.Team1;
        newRoomInfo.team2 = _roomInfo.Team2;

        _sb.Clear();
        _sb.Append(FireStoreCollection.MULTIPLAYER_ROOM).Append("/").Append(_roomOptions.RoomName);

        var roomRef = FirebaseFirestore.DefaultInstance.Document(_sb.ToString());

        roomRef.SetAsync(newRoomInfo, SetOptions.MergeAll).ContinueWithOnMainThread(
            updateTask=> 
            {
                Debug.Log($"Room [{_roomOptions.RoomName}] updated");
                ListenRoomInfoChange();
            }
            );
    }

    public void RequestSwitchToTeam(string userId, int switchTo)
    {
        var db = FirebaseFirestore.DefaultInstance;
        _sb.Clear();
        _sb.Append(FireStoreCollection.MULTIPLAYER_ROOM).Append("/").Append(_roomOptions.RoomName);
        var roomRef = db.Document(_sb.ToString());

        db.RunTransactionAsync(
            transaction =>
            {
                return transaction.GetSnapshotAsync(roomRef).ContinueWithOnMainThread(
                    readTask =>
                    {
                        if (readTask.IsFaulted)
                        {
                            Debug.LogError("Fail to read room info", this);
                            return;
                        }
                        var roomData = readTask.Result.ConvertTo<FireStoreRoomData>();
                        var roomDataAfterSwitch = SwitchTeam(userId, roomData, switchTo);

                        transaction.Set(roomRef, roomData, SetOptions.MergeAll);
                    }
                    );
            }).ContinueWithOnMainThread(
            transactionTask => 
            {
                if (transactionTask.IsFaulted)
                {
                    Debug.LogError("Fail to switch team!", this);
                    return;
                }

                Debug.Log("Team switch success!", this);
            });
    }

    private void UpdateReadyPlayerCount(params object[] args)
    {
        var db = FirebaseFirestore.DefaultInstance;

        _sb.Clear();
        _sb.Append(FireStoreCollection.MULTIPLAYER_ROOM).Append("/").Append(_roomOptions.RoomName);
        var roomRef = db.Document(_sb.ToString());

        db.RunTransactionAsync(
            transaction =>
            {
                return transaction.GetSnapshotAsync(roomRef).ContinueWithOnMainThread(
                    readTask =>
                    {
                        if (readTask.IsFaulted)
                        {
                            Debug.LogError("Fail to read room info", this);
                            return;
                        }
                        var roomData = readTask.Result.ConvertTo<FireStoreRoomData>();
                        roomData.readyOnRoyaleCount++;

                        transaction.Set(roomRef, roomData, SetOptions.MergeAll);
                    }
                    );
            }).ContinueWithOnMainThread(
            transactionTask =>
            {
                if (transactionTask.IsFaulted)
                {
                    Debug.LogError("Fail to Update Ready Count", this);
                    return;
                }

                Debug.Log("Update Ready Count success!", this);
            });
    }

    private void ListenRoomInfoChange()
    {
        if (_currentRoomName.Equals(_roomOptions.RoomName))
        {
            return;
        }

        if (_roomInfoChangeListener != null)
        {
            _roomInfoChangeListener.Stop();
        }

        _currentRoomName = _roomOptions.RoomName;

        var roomQuery = FirebaseFirestore.DefaultInstance
            .Collection(FireStoreCollection.MULTIPLAYER_ROOM).WhereEqualTo("roomName", _roomOptions.RoomName);

        _roomInfoChangeListener = roomQuery.Listen(
            changedRoomData =>
            {
                var changes = changedRoomData.GetChanges();
                foreach(DocumentChange change in changes)
                {
                    _onRoomPlayerDataChangedFromDb?.Raise(change.Document.ConvertTo<FireStoreRoomData>());
                }
            });
    }

    private void ListenRoomPosPicksChange()
    {
        var sb = new StringBuilder();
        sb.Append(RTDBRoomPath.ROOMS_PATH).Append(_roomOptions.RoomName).Append("/").Append(RTDBRoomPath.POS_1_PICKS_KEY);

        _dbAccessor.GetDataRef(sb.ToString()).ValueChanged += (obj, change) =>
        {

        };
    }

    private FireStoreRoomData SwitchTeam(string userId, FireStoreRoomData currentRoomData, int switchTo)
    {
        List<string> teamToSwitch = switchTo == 1 ? currentRoomData.team1 : currentRoomData.team2;
        List<string> currentTeam = switchTo == 1 ? currentRoomData.team2 : currentRoomData.team1;

        if (teamToSwitch.Count >= _roomOptions.MaxPlayersPerTeam)
        {
            return null;
        }

        if (!currentTeam.Contains(userId))
        {
            return null;
        }

        teamToSwitch.Add(userId);
        currentTeam.Remove(userId);

        return currentRoomData;
    }

    private void OnEnable()
    {
        _onRoomPlayerDataChangedFromLocal.Subcribe(UpdateRoomPlayerInfoToDB);
        _onPlayerReady.Subcribe(UpdateReadyPlayerCount);
    }

    private void OnDisable()
    {
        _onRoomPlayerDataChangedFromLocal.Unsubcribe(UpdateRoomPlayerInfoToDB);
        _onPlayerReady.Unsubcribe(UpdateReadyPlayerCount);
    }

    private void OnApplicationQuit()
    {
        if (_roomInfoChangeListener != null)
        {
            _roomInfoChangeListener.Stop();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomInfos", menuName = "NFQ/Multiplayer/Create RoomInfos")]
public class RoomInfoSO : ScriptableObject
{
    [SerializeField]
    private RoomOptionsSO _roomOption;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onRoomPlayerChangedFromLocal;
    [SerializeField]
    private GameEvent _onLocalRoomPlayerModified;
    [SerializeField]
    private GameEvent _onAllPlayerReady;

    [Header("Inspec")]
    [SerializeField]
    private List<string> _team1List;
    [SerializeField]
    private List<string> _team2List;
    [SerializeField]
    private int _readyCount;

    private int ReadyCount
    {
        set
        {
            _readyCount = value;

            if (_readyCount == PlayerCount)
            {
                _onAllPlayerReady?.Raise();
            }
        }
    }

    public List<string> Team1
    {
        get
        {
            return _team1List;
        }
    }

    public List<string> Team2
    {
        get
        {
            return _team2List;
        }
    }

    public int PlayerCount
    {
        get
        {
            return _team1List.Count + _team2List.Count;
        }
    }

    public void ResetRoomInfo()
    {
        _team1List.Clear();
        _team2List.Clear();
    }

    public void CreateNewRoomInfo(bool notifyChanged = false)
    {
        ResetRoomInfo();
        if (notifyChanged)
        {
            UpdateRoomPlayerToDB();
        }

        _onLocalRoomPlayerModified?.Raise();
    }

    public string GetPlayerIdAtPos(int pos)
    {
        int posInList = (pos - 1) % _roomOption.MaxPlayersPerTeam;

        if (pos > _roomOption.MaxPlayersPerTeam)
        {
            if (_team2List.Count == 0 || _team2List.Count < posInList + 1)
            {
                return "";
            }
            return _team2List[posInList];
        }

        if (_team1List.Count == 0 || _team1List.Count < posInList + 1)
        {
            return "";
        }

        return _team1List[posInList];
    }

    public void AddToTeam1(string playerId, bool notifyChanged = false)
    {
        if (_team1List.Count >= _roomOption.MaxPlayersPerTeam)
        {
            return;
        }

        _team1List.Add(playerId);

        if (notifyChanged)
        {
            UpdateRoomPlayerToDB();
        }

        _onLocalRoomPlayerModified?.Raise();
    }

    public void AddToTeam2(string playerId, bool notifyChanged = false)
    {
        if (_team2List.Count >= _roomOption.MaxPlayersPerTeam)
        {
            return;
        }
        _team2List.Add(playerId);

        if (notifyChanged)
        {
            UpdateRoomPlayerToDB();
        }

        _onLocalRoomPlayerModified?.Raise();
    }

    public void AddToAvailableSlot(string playerId, bool notifyChanged = false)
    {
        if (_team1List.Count < _roomOption.MaxPlayersPerTeam)
        {
            _team1List.Add(playerId);

            if (notifyChanged)
            {
                UpdateRoomPlayerToDB();
            }

            _onLocalRoomPlayerModified?.Raise();
            return;
        }

        if (_team2List.Count < _roomOption.MaxPlayersPerTeam)
        {
            _team2List.Add(playerId);
            if (notifyChanged)
            {
                UpdateRoomPlayerToDB();
            }
        }

        _onLocalRoomPlayerModified?.Raise();
    }

    public void DropPlayer(string playerId, bool notifyChanged = false)
    {
        _team1List.Remove(playerId);
        _team2List.Remove(playerId);

        if (notifyChanged)
        {
            UpdateRoomPlayerToDB();
        }

        _onLocalRoomPlayerModified?.Raise();
    }

    public void CreateInfoByDbData(FireStoreRoomData roomData)
    {
        ResetRoomInfo();

        _team1List = roomData.team1;
        _team2List = roomData.team2;
        ReadyCount = roomData.readyOnRoyaleCount;

        _onLocalRoomPlayerModified?.Raise();
    }

    private void UpdateRoomPlayerToDB()
    {
        _onRoomPlayerChangedFromLocal?.Raise();
    }
}

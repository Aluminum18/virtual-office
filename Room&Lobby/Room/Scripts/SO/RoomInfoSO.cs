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
    [SerializeField]
    private List<int> _pos1Picks;
    [SerializeField]
    private List<int> _pos2Picks;
    [SerializeField]
    private List<int> _pos3Picks;
    [SerializeField]
    private List<int> _pos4Picks;
    [SerializeField]
    private List<int> _pos5Picks;
    [SerializeField]
    private List<int> _pos6Picks;

    public int ReadyCount
    {
        set
        {
            _readyCount = value;

            if (_readyCount >= PlayerCount)
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
        set
        {
            _team1List = value;
        }
    }
    public List<string> Team2
    {
        get
        {
            return _team2List;
        }
        set
        {
            _team2List = value;
        }
    }

    public List<int> Pos1Picks
    {
        get
        {
            return _pos1Picks;
        }
        set
        {
            _pos1Picks = value;
        }
    }
    public List<int> Pos2Picks
    {
        get
        {
            return _pos2Picks;
        }
        set
        {
            _pos2Picks = value;
        }
    }
    public List<int> Pos3Picks
    {
        get
        {
            return _pos3Picks;
        }
        set
        {
            _pos3Picks = value;
        }
    }
    public List<int> Pos4Picks
    {
        get
        {
            return _pos4Picks;
        }
        set
        {
            _pos4Picks = value;
        }
    }
    public List<int> Pos5Picks
    {
        get
        {
            return _pos5Picks;
        }
        set
        {
            _pos5Picks = value;
        }
    }
    public List<int> Pos6Picks
    {
        get
        {
            return _pos6Picks;
        }
        set
        {
            _pos6Picks = value;
        }
    }

    public int PlayerCount
    {
        get
        {
            return _team1List.Count + _team2List.Count;
        }
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

    public int GetPlayerPos(string userId)
    {
        if (_team1List.Contains(userId))
        {
            return _team1List.IndexOf(userId) + 1;
        }

        if (_team2List.Contains(userId))
        {
            return _team2List.IndexOf(userId) + _roomOption.MaxPlayersPerTeam + 1;
        }

        Debug.LogError($"Cannot find position of userId [{userId}]", this);
        return -1;
    }

    public int GetTeam(string userId)
    {
        if (_team1List.Contains(userId))
        {
            return 1;
        }
        else if (_team2List.Contains(userId))
        {
            return 2;
        }

        return 0;
    }
}

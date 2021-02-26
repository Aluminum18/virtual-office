using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.Events;
using Photon.Pun;

public class RoomInfoAccessorRTDB : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private RoomOptionsSO _roomOptions;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private RealTimeDBAccessor _dbAccessor;

    [SerializeField]
    private StringVariable _thisUserId;
    [SerializeField]
    private IntegerListVariable _thisPickedSkills;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onRoomFull;
    [Header("Event outs")]
    [SerializeField]
    private GameEvent _onRoomInfoChanged;

    private static string _currentRoomName = "";

    public void RequestJoinRoom()
    {
        var sb = new StringBuilder();
        sb.Append(RTDBRoomPath.ROOMS_PATH).Append(_roomOptions.RoomName);
        var roomRef = _dbAccessor.GetDataRef(sb.ToString());

        roomRef.RunTransaction(
            roomData =>
            {
                if (!(roomData.Value is Dictionary<string, object> roomDataValue))
                {
                    roomDataValue = new Dictionary<string, object>();
                    roomDataValue[RTDBRoomPath.ROOM_NAME_KEY] = _roomOptions.RoomName;
                    roomDataValue[RTDBRoomPath.MAX_PLAYERS_PER_TEAM_KEY] = _roomOptions.MaxPlayersPerTeam;
                }

                roomDataValue.TryGetValue(RTDBRoomPath.TEAM_ONE_KEY, out var team1);
                if (!(team1 is List<object> team1Value))
                {
                    team1Value = new List<object>();
                }

                string posKey = "";
                if (team1Value.Count >= _roomOptions.MaxPlayersPerTeam)
                {
                    roomDataValue.TryGetValue(RTDBRoomPath.TEAM_TWO_KEY, out var team2);
                    if (!(team2 is List<object> team2Value))
                    {
                        team2Value = new List<object>();
                    }

                    if (team2Value.Count >= _roomOptions.MaxPlayersPerTeam)
                    {
                        _onRoomFull.Invoke();
                        RemoveRoomChangeListener(_currentRoomName);
                        return TransactionResult.Abort();
                    }

                    if (!team2Value.Contains(_thisUserId.Value))
                    {
                        team2Value.Add(_thisUserId.Value);
                    }
                    roomDataValue[RTDBRoomPath.TEAM_TWO_KEY] = team2Value;

                    posKey = GetPosKey(team2Value.Count + 3);

                }
                else
                {
                    if (!team1Value.Contains(_thisUserId.Value))
                    {
                        team1Value.Add(_thisUserId.Value);
                    }
                    roomDataValue[RTDBRoomPath.TEAM_ONE_KEY] = team1Value;
                    posKey = GetPosKey(team1Value.Count);
                }

                roomDataValue[posKey] = _thisPickedSkills.List;

                roomData.Value = roomDataValue;

                ListenRoomInfoChange();
                return TransactionResult.Success(roomData);
            });
    }

    public void RequestLeaveRoom(string userId)
    {
        RemoveRoomChangeListener(_roomOptions.RoomName);

        var sb = new StringBuilder();
        sb.Append(RTDBRoomPath.ROOMS_PATH).Append(_roomOptions.RoomName);

        var roomRef = _dbAccessor.GetDataRef(sb.ToString());
        roomRef.RunTransaction(
            roomData =>
            {
                if (!(roomData.Value is Dictionary<string, object> roomDataValue))
                {
                    roomDataValue = new Dictionary<string, object>();
                    roomDataValue[RTDBRoomPath.ROOM_NAME_KEY] = _roomOptions.RoomName;
                    roomDataValue[RTDBRoomPath.MAX_PLAYERS_PER_TEAM_KEY] = _roomOptions.MaxPlayersPerTeam;
                }

                roomDataValue.TryGetValue(RTDBRoomPath.TEAM_ONE_KEY, out var team1);
                if (!(team1 is List<object> team1Value))
                {
                    team1Value = new List<object>();
                }

                roomDataValue.TryGetValue(RTDBRoomPath.TEAM_TWO_KEY, out var team2);
                if (!(team2 is List<object> team2Value))
                {
                    team2Value = new List<object>();
                }

                if (team1Value.Count + team2Value.Count <= 1)
                {
                    roomData.Value = null;
                    return TransactionResult.Success(roomData);
                }

                if (team1Value.Contains(userId))
                {
                    team1Value.Remove(userId);
                }
                else
                {
                    team2Value.Remove(userId);
                }

                roomData.Value = roomDataValue;

                return TransactionResult.Success(roomData);
            });
    }

    public void RequestSwitchTeam(string userId, int switchTo)
    {
        var sb = new StringBuilder();

        sb.Append(RTDBRoomPath.ROOMS_PATH).Append(_roomOptions.RoomName);
        var roomRef = _dbAccessor.GetDataRef(sb.ToString());
        roomRef.RunTransaction(
            roomData =>
            {
                string switchToTeamKey = switchTo == 1 ? RTDBRoomPath.TEAM_ONE_KEY : RTDBRoomPath.TEAM_TWO_KEY;
                string currentTeamKey = switchTo == 1 ? RTDBRoomPath.TEAM_TWO_KEY : RTDBRoomPath.TEAM_ONE_KEY;

                if (!(roomData.Value is Dictionary<string, object> roomDataValue))
                {
                    roomDataValue = new Dictionary<string, object>();
                }

                if (!roomDataValue.ContainsKey(switchToTeamKey))
                {
                    roomDataValue[switchToTeamKey] = new List<object>();
                }

                if (!roomDataValue.ContainsKey(currentTeamKey))
                {
                    roomDataValue[currentTeamKey] = new List<object>();
                }

                var switchToTeam = roomDataValue[switchToTeamKey] as List<object>;
                if (switchToTeam.Count >= _roomOptions.MaxPlayersPerTeam)
                {
                    return TransactionResult.Abort();
                }

                var currentTeam = roomDataValue[currentTeamKey] as List<object>;

                switchToTeam.Add(userId);
                int newPos = switchToTeam.Count + (switchTo != 1 ? 3 : 0);

                int oldPos = currentTeam.IndexOf(userId) + (switchTo == 1 ? 3 : 0) + 1;
                currentTeam.Remove(userId);

                string newPosKey = GetPosKey(newPos);
                string oldPosKey = GetPosKey(oldPos);

                if (!roomDataValue.ContainsKey(newPosKey))
                {
                    roomDataValue[newPosKey] = new List<object>();
                }

                if (!roomDataValue.ContainsKey(oldPosKey))
                {
                    roomDataValue[oldPosKey] = new List<object>();
                }

                roomDataValue[newPosKey] = new List<object>((List<object>)roomDataValue[oldPosKey]);
                ((List<object>)roomDataValue[oldPosKey]).Clear();

                roomData.Value = roomDataValue;

                return TransactionResult.Success(roomData);
            }
            );
    }

    public void RequestSetReadyInRoyale()
    {
        var sb = new StringBuilder();

        sb.Append(RTDBRoomPath.ROOMS_PATH).Append(_roomOptions.RoomName).Append("/").Append(RTDBRoomPath.READY_ON_ROYALE_COUNT_KEY);
        var readyRef = _dbAccessor.GetDataRef(sb.ToString());
        readyRef.RunTransaction(
            readyData =>
            {
                if (!(readyData.Value is long readyValue))
                {
                    readyValue = 0;
                }
                readyData.Value = readyValue + 1;

                return TransactionResult.Success(readyData);
            });
    }

    public void SubmitPickedSkill()
    {
        string posKey = GetPosKey(_roomInfo.GetPlayerPos(_thisUserId.Value));

        var sb = new StringBuilder();
        sb.Append(RTDBRoomPath.ROOMS_PATH).Append(_roomOptions.RoomName).Append("/").Append(posKey);

        _dbAccessor.UpdateData(sb.ToString(), _thisPickedSkills.List);
    }

    private void ListenRoomInfoChange()
    {
        if (_currentRoomName.Equals(_roomOptions.RoomName))
        {
            return;
        }

        RemoveRoomChangeListener(_currentRoomName);

        var sb = new StringBuilder();
        sb.Append(RTDBRoomPath.ROOMS_PATH).Append(_roomOptions.RoomName);
        var roomRef = _dbAccessor.GetDataRef(sb.ToString());

        roomRef.ValueChanged += HandleRoomInfoChange;

        _currentRoomName = _roomOptions.RoomName;
    }

    private void RemoveRoomChangeListener(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            return;
        }

        var sb = new StringBuilder();
        sb.Append(RTDBRoomPath.ROOMS_PATH).Append(roomName);
        _dbAccessor.GetDataRef(sb.ToString()).ValueChanged -= HandleRoomInfoChange;
    }

    private void HandleRoomInfoChange(object sender, ValueChangedEventArgs change)
    {
        if (change.DatabaseError != null)
        {
            Debug.LogError(change.DatabaseError.Message);
            return;
        }

        var changeRoomData = JsonUtility.FromJson<RTDBRoomData>(change.Snapshot.GetRawJsonValue());

        _roomInfo.Team1 = changeRoomData.teamOne;
        _roomInfo.Team2 = changeRoomData.teamTwo;

        _roomInfo.ReadyCount = changeRoomData.readyOnRoyaleCount;

        _roomInfo.Pos1Picks = changeRoomData.pos1Picks;
        _roomInfo.Pos2Picks = changeRoomData.pos2Picks;
        _roomInfo.Pos3Picks = changeRoomData.pos3Picks;
        _roomInfo.Pos4Picks = changeRoomData.pos4Picks;
        _roomInfo.Pos5Picks = changeRoomData.pos5Picks;
        _roomInfo.Pos6Picks = changeRoomData.pos6Picks;

        _onRoomInfoChanged.Raise();
    }

    private string GetPosKey(int pos)
    {
        switch (pos)
        {
            case 1:
                return RTDBRoomPath.POS_1_PICKS_KEY;
            case 2:
                return RTDBRoomPath.POS_2_PICKS_KEY;
            case 3:
                return RTDBRoomPath.POS_3_PICKS_KEY;
            case 4:
                return RTDBRoomPath.POS_4_PICKS_KEY;
            case 5:
                return RTDBRoomPath.POS_5_PICKS_KEY;
            case 6:
                return RTDBRoomPath.POS_6_PICKS_KEY;
            default:
                Debug.LogError($"invalid pos [{pos}]", this);
                return "";
        }
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(_currentRoomName))
        {
            return;
        }

        ListenRoomInfoChange();
    }

    private void OnDisable()
    {
        RemoveRoomChangeListener(_currentRoomName);
    }

    private void OnApplicationQuit()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // TODO: remove only when all players leave
        RemoveRoomChangeListener(_roomOptions.RoomName);
        _dbAccessor.RemoveAChild(RTDBRoomPath.ROOMS_PATH + _roomOptions.RoomName);
    }
}

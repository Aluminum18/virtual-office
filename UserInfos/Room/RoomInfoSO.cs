using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomInfos", menuName = "NFQ/Multiplayer/Create RoomInfos")]
public class RoomInfoSO : ScriptableObject
{
    [SerializeField]
    private RoomSO _roomOption;

    [SerializeField]
    private List<string> _team1List;
    [SerializeField]
    private List<string> _team2List;

    public void AddToTeam1(string playerId)
    {
        if (_team1List.Count >= _roomOption.MaxPlayersPerTeam)
        {
            return;
        }

        _team1List.Add(playerId);
    }

    public void AddToTeam2(string playerId)
    {
        if (_team1List.Count >= _roomOption.MaxPlayersPerTeam)
        {
            return;
        }
    }
}

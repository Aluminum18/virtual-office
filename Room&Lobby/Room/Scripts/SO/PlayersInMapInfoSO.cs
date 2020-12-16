using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersInMapInfo", menuName = "NFQ/Multiplayer/Create PlayersInMapInfo")]
[System.Serializable]
public class PlayersInMapInfoSO : ScriptableObject
{
    [SerializeField]
    private List<PlayerInMapInfo> _players;

    public PlayerInMapInfo GetPlayerInfo(int pos)
    {
        if (_players.Count < pos - 1)
        {
            Debug.LogError($"Invalid position [{pos}]");
            return null;
        }

        return _players[pos - 1];
    }
}

[System.Serializable]
public class PlayerInMapInfo
{
    public FloatVariable Hp;
}

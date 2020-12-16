using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private PlayersInMapInfoSO _playersInMapInfo;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private StringVariable _userId;

    [SerializeField]
    private HPBar _hpBar;

    public void MapUIToSO()
    {
        int pos = _roomInfo.GetPlayerPos(_userId.Value);
        if (pos == -1)
        {
            return;
        }

        var playerInfo =_playersInMapInfo.GetPlayerInfo(pos);
        if (playerInfo == null)
        {
            return;
        }

        _hpBar.SetHpVariable(playerInfo.Hp);

    }
}

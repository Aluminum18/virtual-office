using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButtons : MonoBehaviour
{
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private StringVariable _userId;
    [SerializeField]
    private RoomInfoAccessor _roomDbAccessor;

    public void ChangeToTeam1()
    {
        _roomDbAccessor.RequestSwitchToTeam(_userId.Value, 1);
    }

    public void ChangeToTeam2()
    {
        _roomDbAccessor.RequestSwitchToTeam(_userId.Value, 2);
    }
}

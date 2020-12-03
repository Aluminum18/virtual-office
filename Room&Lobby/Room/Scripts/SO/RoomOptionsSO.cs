using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomOption", menuName = "NFQ/Multiplayer/Create RoomOptions")]
public class RoomOptionsSO : ScriptableObject
{
    public string RoomName = "DefaultRoomName";
    public byte MaxPlayersPerTeam = 3;
    public string Password;
    public bool IsVisible = true;

    public void Reset()
    {
        RoomName = "";
    }
}

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDataSync : MonoBehaviour, IOnEventCallback
{
    [SerializeField]
    private StringVariable _thisUserId;
    [SerializeField]
    private RoomInfoSO _roomInfo;
    [SerializeField]
    private List<IntegerListVariable> _allPlayerPickedSkills;

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != PhotonEventCode.ROOM_PICKED_SKILLS_CHANGED)
        {
            return;
        }

        var eventData = (object[])photonEvent.CustomData;
        int pos = (int)eventData[0];
        var changedPickedSkills = (List<int>)eventData[1];

        var needUpdate = _allPlayerPickedSkills[pos];
        needUpdate.AssignNew(changedPickedSkills);
    }

    public void NotifyPickedSkillsChange()
    {
        int pos = _roomInfo.GetPlayerPos(_thisUserId.Value);
        if (pos == -1)
        {
            Debug.LogError($"invalid pos [{pos}]", this);
            return;
        }

        List<int> pickedSkills = _allPlayerPickedSkills[pos - 1].List;


        object[] data = new object[]
        {
            pos,
            pickedSkills
        };

        SendPunEvent(PhotonEventCode.ROOM_PICKED_SKILLS_CHANGED, data);
    }

    private void SendPunEvent(byte eventCode, object data)
    {
        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent(eventCode, data, eventOptions, sendOptions);
    }

}

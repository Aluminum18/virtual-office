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

        if (pos < 0)
        {
            Debug.LogError($"invalid pos [{pos}]", this);
            return;
        }

        var changedPickedSkills = (int[])eventData[1];

        var needUpdate = _allPlayerPickedSkills[pos - 1];
        needUpdate.AssignNew(new List<int>(changedPickedSkills));
    }

    public void NotifyPickedSkillsChange()
    {
        int pos = _roomInfo.GetPlayerPos(_thisUserId.Value);
        if (pos == -1)
        {
            Debug.LogError($"invalid pos [{pos}]", this);
            return;
        }

        int[] pickedSkills = _allPlayerPickedSkills[pos - 1].List.ToArray();


        object[] data = new object[]
        {
            pos,
            pickedSkills
        };

        SendPunEvent(PhotonEventCode.ROOM_PICKED_SKILLS_CHANGED, data);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
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

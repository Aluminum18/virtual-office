using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunEventSender : MonoBehaviour
{
    protected void SendEvent(byte eventCode, object eventData)
    {
        if (!CheckCondition())
        {
            return;
        }

        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };

        Debug.Log($"Send event [{eventCode}]");
        PhotonNetwork.RaiseEvent(eventCode, eventData, eventOptions, sendOptions);
    }

    protected virtual bool CheckCondition()
    {
        return true;
    }
}

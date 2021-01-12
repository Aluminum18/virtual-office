using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunEventSender : MonoBehaviour
{
    public void SendEvent(byte eventCode, ReceiverGroup receivers, params object[] eventData)
    {
        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            Receivers = receivers,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };

        Debug.Log($"Send event [{eventCode}]");
        PhotonNetwork.RaiseEvent(eventCode, eventData, eventOptions, sendOptions);
    }
}

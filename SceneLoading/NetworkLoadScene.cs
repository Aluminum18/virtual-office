using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LoadNewSceneRequester))]
public class NetworkLoadScene : MonoBehaviour, IOnEventCallback
{
    private LoadNewSceneRequester _sceneLoader;

    public void RequestLoadScene(string sceneName)
    {
        string nextSceneName = sceneName;

        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent(PhotonEventCode.LOAD_SCENE, nextSceneName, eventOptions, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != PhotonEventCode.LOAD_SCENE)
        {
            return;
        }

        string nextSceneName = (string)photonEvent.CustomData;

        _sceneLoader.LoadNextSceneWithName(nextSceneName);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        _sceneLoader = GetComponent<LoadNewSceneRequester>();
    }
}

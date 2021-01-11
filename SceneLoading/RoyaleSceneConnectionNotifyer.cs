using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoyaleSceneConnectionNotifyer : MonoBehaviour
{
    [Header("Events in")]
    [SerializeField]
    private GameEvent _onSceneOpened;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onPlayerReady;

    private void OnEnable()
    {
        _onSceneOpened.Subcribe(NotifyOnScene);
    }

    private void OnDisable()
    {
        _onSceneOpened.Unsubcribe(NotifyOnScene);
    }

    private void NotifyOnScene(params object[] args)
    {
        _onPlayerReady.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoyaleSceneConnectionNotifyer : MonoBehaviour
{
    [Header("Events in")]
    [SerializeField]
    private GameEvent _onSceneOpened;

    [Header("Events out")]
    [SerializeField]
    private GameEvent _onPlayerReady;

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
        _onPlayerReady?.Raise();
    }
}

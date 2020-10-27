using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInputHandler : MonoBehaviour
{
    [Header("Events in")]
    [SerializeField]
    private GameEvent _onTouchedWorld;

    [Header("Config")]
    [SerializeField]
    private AgentMoving _agentMoving;

    private void OnEnable()
    {
        _onTouchedWorld.Subcribe(Move);
    }

    private void OnDisable()
    {
        _onTouchedWorld.Unsubcribe(Move);
    }

    private void Move(params object[] destination)
    {
        _agentMoving.MoveTo((Vector3)destination[0]);
    }

}

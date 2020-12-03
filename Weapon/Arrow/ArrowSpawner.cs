using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowSpawner : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private StringVariable _charaterState;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onSpawnArrow;

    [SerializeField]
    private Vector3Variable _aimSpot;
    [SerializeField]
    private ObjectPool _arrowPool;
    [SerializeField]
    private float _arrowSpeed;

    public void FireArrow()
    {
        GameObject arrow = _arrowPool.Spawn();
        var arrowMoving = arrow.GetComponent<ArrowMoving>();

        _onSpawnArrow.Invoke();

        if (_charaterState.Value.Equals(CharacterState.STATE_READY_ATTACK))
        {
            arrowMoving.HeadTo(_aimSpot.Value);
        }

        arrowMoving.MoveForward(_arrowSpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowSpawner : ProjectileSpawner
{
    [Header("Reference - assigned at runtime")]
    [SerializeField]
    private Vector3Variable _aimSpot;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onSpawnArrow;

    public void SetAimSpotInput(Vector3Variable aimSpot)
    {
        _aimSpot = aimSpot;
    }

    public void SetState(StringVariable state)
    {
    }

    public void FireArrow()
    {
        GameObject arrow = SpawnProjectile(_aimSpot.Value);
    }
}

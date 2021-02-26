using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetableProjectileSpawner : ProjectileSpawner
{
    [Header("Reference - assigned at runtime")]
    [SerializeField]
    protected Vector3Variable _aimSpot;

    public void SetAimSpotInput(Vector3Variable aimSpot)
    {
        _aimSpot = aimSpot;
    }

    public void SetState(StringVariable state)
    {
    }

    public GameObject FireArrowToAimSpot()
    {
        return SpawnProjectile(_aimSpot.Value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrNadeSpawner : ProjectileSpawner
{
    [SerializeField]
    private Vector3Variable _aimSpot;

    public void SpawnArrnade()
    {
        SpawnProjectile(_aimSpot.Value, _projectileSpeed, MovingPath.Straight);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrNadeSpawner : ProjectileSpawner
{
    [Header("Reference - assign at runtime")]
    [SerializeField]
    private Vector3Variable _aimSpot;

    private GameObject _spawnedArrNade;
   
    public void SpawnArrnade()
    {
        _spawnedArrNade = SpawnProjectile(_aimSpot.Value);
    }

    public void Setup(Vector3Variable aimSpot)
    {
        _aimSpot = aimSpot;
    }

    public void ExploseArrnade()
    {
        if (_spawnedArrNade == null)
        {
            return;
        }

        var arrnade = _spawnedArrNade.GetComponent<ArrNade>();
        if (arrnade == null)
        {
            return;
        }

        arrnade.Explose();
    }
}

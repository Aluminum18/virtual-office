using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrNadeSpawner : TargetableProjectileSpawner
{
    private GameObject _spawnedArrNade;
   
    public void SpawnArrnade()
    {
        _spawnedArrNade = FireArrowToAimSpot();
    }

    public void Setup(Vector3Variable aimSpot)
    {
        SetAimSpotInput(aimSpot);
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

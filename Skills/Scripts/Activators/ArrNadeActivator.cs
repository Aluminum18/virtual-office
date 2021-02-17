using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrNadeActivator : SkillActivator
{
    [SerializeField]
    private ArrNadeSpawner _arrnadeSpawner;


    public override void FirstState()
    {
        _arrnadeSpawner.SpawnArrnade();
    }

    public override void SecondState()
    {
        _arrnadeSpawner.ExploseArrnade();
    }

    public override void Setup(params object[] args)
    {
        if (!(args[0] is Vector3Variable aimSpot))
        {
            Debug.LogError("Aimspot for arrnade activator is null", this);
            return;
        }
        _arrnadeSpawner.Setup(aimSpot);
    }
}

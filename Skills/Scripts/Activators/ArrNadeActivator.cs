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
        _arrnadeSpawner.Owner = Owner;
        _arrnadeSpawner.Team = Team;

        if (!(args[0] is Vector3Variable aimSpot))
        {
            Debug.LogError($"Aimspot for [{gameObject.name}] is null", this);
            return;
        }

        if (!(args[1] is int damage))
        {
            Debug.LogError($"damage for [{gameObject.name}] is invalid", this);
            return;
        }

        _arrnadeSpawner.Setup(aimSpot);
        _arrnadeSpawner.ProjectileDamage = damage;
    }
}

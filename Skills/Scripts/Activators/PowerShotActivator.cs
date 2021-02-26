using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShotActivator : SkillActivator
{
    [SerializeField]
    private TargetableProjectileSpawner _spawner;

    public override void FirstState()
    {
        _spawner.FireArrowToAimSpot();
    }

    public override void Setup(params object[] args)
    {
        if (!(args[0] is Vector3Variable aimSpot)) // aimspot
        {
            Debug.LogError("aimspot for PowerShot is invalid", this);
            return;
        }
        _spawner.SetAimSpotInput(aimSpot);
        _spawner.Owner = Owner;
        _spawner.Team = Team;

        if (!(args[1] is float delayTime)) // reload time of using weapon
        {
            Debug.LogError("reload time for PowerShot is invalid", this);
            return;
        }

        if (!(args[2] is int damage))
        {
            Debug.LogError("damage for[{gameObject.name}] is invalid", this);
            return;
        }

        _spawner.ProjectileDamage = damage;

        _skillSO.CastTime = delayTime;
    }
}

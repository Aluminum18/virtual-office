using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShotActivator : SkillActivator
{
    [SerializeField]
    private TargetableProjectileSpawner _spawner;

    public override void FirstState()
    {
        _spawner.FireArrow();
    }

    public override void Setup(params object[] args)
    {
        if (!(args[0] is Vector3Variable aimSpot)) // aimspot
        {
            Debug.LogError("aimspot for PowerShot is invalid", this);
            return;
        }
        _spawner.SetAimSpotInput(aimSpot);

        if (!(args[1] is float delayTime)) // reload time of using weapon
        {
            Debug.LogError("reload time for PowerShot is invalid", this);
            return;
        }
        _skillSO.CastTime = delayTime;
    }
}

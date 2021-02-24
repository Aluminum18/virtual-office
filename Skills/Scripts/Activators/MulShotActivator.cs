using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MulShotActivator : SkillActivator
{
    [SerializeField]
    private TargetableProjectileSpawner _spawner;

    [SerializeField]
    private float _angle;
    [SerializeField]
    private float _delayBetween = 0.1f;

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

    public override void FirstState()
    {
        _spawner.FireArrowToAimSpot();
    }
}

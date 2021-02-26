using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTargetableWeapon : BaseWeapon
{
    [Header("Runtime Reference")]
    [SerializeField]
    private Vector3Variable _aimTo;

    [Header("Config")]
    [SerializeField]
    private TargetableProjectileSpawner _arrowSpawner;

    private void OnEnable()
    {
        _arrowSpawner.SetAimSpotInput(_aimTo);
    }

    public void SetTarget(Vector3Variable target)
    {
        _arrowSpawner.SetAimSpotInput(target);
    }

    public void SetTeamAndOwner(int team, string owner)
    {
        _arrowSpawner.Team = team;
        _arrowSpawner.Owner = owner;
    }

    public override void AttackImmediately()
    {
        base.AttackImmediately();
        _arrowSpawner.FireArrowToAimSpot();
    }
}
